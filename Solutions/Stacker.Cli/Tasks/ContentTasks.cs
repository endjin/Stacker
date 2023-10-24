// <copyright file="ContentTasks.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NodaTime;
using Stacker.Cli.Configuration;
using Stacker.Cli.Contracts.Buffer;
using Stacker.Cli.Contracts.Configuration;
using Stacker.Cli.Contracts.Formatters;
using Stacker.Cli.Contracts.Tasks;
using Stacker.Cli.Converters;
using Stacker.Cli.Domain.Publication;
using Stacker.Cli.Domain.Universal;

namespace Stacker.Cli.Tasks;

public class ContentTasks : IContentTasks
{
    private readonly IBufferClient bufferClient;
    private readonly IStackerSettingsManager settingsManager;

    public ContentTasks(IBufferClient bufferClient, IStackerSettingsManager settingsManager)
    {
        this.bufferClient = bufferClient;
        this.settingsManager = settingsManager;
    }

    public async Task BufferContentItemsAsync<TContentFormatter>(string contentFilePath, string profilePrefix, string profileName, PublicationPeriod publicationPeriod, DateTime fromDate, DateTime toDate, int itemCount)
        where TContentFormatter : class, IContentFormatter, new()
    {
        TContentFormatter formatter = new();

        string profileKey = profilePrefix + profileName;

        StackerSettings settings = this.settingsManager.LoadSettings(nameof(StackerSettings));

        if (settings.BufferProfiles.ContainsKey(profileKey))
        {
            string profileId = settings.BufferProfiles[profileKey];

            Console.WriteLine($"Buffer Profile: {profileKey} = {profileId}");
            Console.WriteLine($"Loading: {contentFilePath}");

            IEnumerable<ContentItem> contentItems = await this.LoadContentItemsAsync(contentFilePath, publicationPeriod, fromDate, toDate, itemCount).ConfigureAwait(false);
            IEnumerable<string> formattedContentItems = formatter.Format("social", profileName, contentItems);

            await this.bufferClient.UploadAsync(formattedContentItems, profileId).ConfigureAwait(false);
        }
        else
        {
            Console.WriteLine($"Settings for {profileKey} not found. Please check your Stacker configuration.");
        }
    }

    public async Task<IEnumerable<ContentItem>> LoadContentItemsAsync(string contentFilePath, PublicationPeriod publicationPeriod, DateTime fromDate, DateTime toDate, int itemCount)
    {
        IEnumerable<ContentItem> content = JsonConvert.DeserializeObject<IEnumerable<ContentItem>>(await File.ReadAllTextAsync(contentFilePath).ConfigureAwait(false));

        if (publicationPeriod != PublicationPeriod.None)
        {
            DateInterval dateRange = new PublicationPeriodConverter().Convert(publicationPeriod);

            content = content.Where(p => (LocalDate.FromDateTime(p.PublishedOn.LocalDateTime) >= dateRange.Start) && (LocalDate.FromDateTime(p.PublishedOn.LocalDateTime) <= dateRange.End));
        }
        else
        {
            // if fromDate is specified, but toDate isn't, set toDate to now. If toDate is specified, use that.
            if (fromDate != DateTime.MinValue)
            {
                if (toDate == DateTime.MinValue)
                {
                    toDate = DateTime.Now;
                }
                else
                {
                    if (toDate.ToString("HH:mm:ss") == "00:00:00")
                    {
                        toDate = new(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                    }
                }

                content = content.Where(p => p.PublishedOn.LocalDateTime >= fromDate && p.PublishedOn.LocalDateTime <= toDate);
            }
            else
            {
                // if fromDate isn't specified, but toDate is
                if (toDate != DateTime.MinValue)
                {
                    content = content.Where(p => p.PublishedOn.LocalDateTime >= fromDate && p.PublishedOn.LocalDateTime <= toDate);
                }
            }
        }

        // Sort so that content with the shortest lifespan are first.
        content = content.OrderBy(p => p.PromoteUntil).ToList();

        var contentItems = content.ToList();

        if (itemCount == 0)
        {
            itemCount = contentItems.Count;
        }

        Console.WriteLine($"Total Posts: {contentItems.Count}");
        Console.WriteLine($"Promoting first: {itemCount}");

        return contentItems.Take(itemCount);
    }
}