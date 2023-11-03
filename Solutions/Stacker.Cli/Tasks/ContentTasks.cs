// <copyright file="ContentTasks.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using NodaTime;

using Spectre.Console;
using Spectre.IO;

using Stacker.Cli.Configuration;
using Stacker.Cli.Contracts.Buffer;
using Stacker.Cli.Contracts.Formatters;
using Stacker.Cli.Contracts.Tasks;
using Stacker.Cli.Converters;
using Stacker.Cli.Domain.Publication;
using Stacker.Cli.Domain.Universal;

namespace Stacker.Cli.Tasks;

public class ContentTasks : IContentTasks
{
    private readonly IBufferClient bufferClient;
    private readonly StackerSettings settings;
    private readonly IHttpClientFactory httpClientFactory;

    public ContentTasks(IBufferClient bufferClient, StackerSettings settings, IHttpClientFactory httpClientFactory)
    {
        this.bufferClient = bufferClient;
        this.settings = settings;
        this.httpClientFactory = httpClientFactory;
    }

    public async Task BufferContentItemsAsync<TContentFormatter>(
        FilePath contentFilePath,
        Uri contentUri,
        string profilePrefix,
        string profileName,
        PublicationPeriod publicationPeriod,
        DateTime fromDate,
        DateTime toDate,
        int itemCount,
        string filterByTag,
        bool whatIf)
        where TContentFormatter : class, IContentFormatter, new()
    {
        TContentFormatter formatter = new();

        string profileKey = profilePrefix + profileName;

        if (this.settings.BufferProfiles.TryGetValue(profileKey, out string profile))
        {
            AnsiConsole.MarkupLineInterpolated($"[yellow1]Buffer Profile:[/] {profileKey} = {profile}");
            AnsiConsole.MarkupLineInterpolated($"[yellow1]Loading:[/] {contentFilePath}");

            IEnumerable<ContentItem> contentItems = await this.LoadContentItemsAsync(contentFilePath, contentUri, publicationPeriod, fromDate, toDate, itemCount, filterByTag).ConfigureAwait(false);
            IEnumerable<string> formattedContentItems = formatter.Format("social", profileName, contentItems, this.settings);

            await this.bufferClient.UploadAsync(formattedContentItems, profile, whatIf).ConfigureAwait(false);
        }
        else
        {
            AnsiConsole.WriteLine($"Settings for {profileKey} not found. Please check your Stacker configuration.");
        }
    }

    public async Task<IEnumerable<ContentItem>> LoadContentItemsAsync(
        FilePath contentFilePath,
        Uri contentUri,
        PublicationPeriod publicationPeriod,
        DateTime fromDate,
        DateTime toDate,
        int itemCount,
        string filterByTag)
    {
        string fileContent = string.Empty;

        if (contentUri is not null || !string.IsNullOrEmpty(contentUri.AbsoluteUri))
        {
            using HttpClient client = this.httpClientFactory.CreateClient();
            fileContent = await client.GetStringAsync(contentUri).ConfigureAwait(false);
        }
        else if (contentFilePath is not null || !string.IsNullOrEmpty(contentFilePath.FullPath))
        {
            fileContent = await File.ReadAllTextAsync(contentFilePath.FullPath).ConfigureAwait(false);
        }

        List<ContentItem> content = JsonSerializer.Deserialize<List<ContentItem>>(fileContent);

        if (publicationPeriod != PublicationPeriod.None)
        {
            DateInterval dateRange = new PublicationPeriodConverter().Convert(publicationPeriod);

            content = content.Where(p => (LocalDate.FromDateTime(p.PublishedOn.LocalDateTime) >= dateRange.Start) && (LocalDate.FromDateTime(p.PublishedOn.LocalDateTime) <= dateRange.End)).ToList();
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
                        toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                    }
                }

                content = content.Where(p => p.PublishedOn.LocalDateTime >= fromDate && p.PublishedOn.LocalDateTime <= toDate).ToList();
            }
            else
            {
                // if fromDate isn't specified, but toDate is
                if (toDate != DateTime.MinValue)
                {
                    content = content.Where(p => p.PublishedOn.LocalDateTime >= fromDate && p.PublishedOn.LocalDateTime <= toDate).ToList();
                }
            }
        }

        foreach (ContentItem contentItem in content)
        {
            // Use TagAliases to convert tags into their canonical form.
            contentItem.Tags = contentItem.Tags?.Select(tag =>
            {
                TagAliases matchedAlias = this.settings.TagAliases.FirstOrDefault(alias => alias.Aliases.Any(a => a == tag));
                return matchedAlias != null ? matchedAlias.Tag : tag.Replace("-", " ").Replace(" ", string.Empty);
            }).ToList();
        }

        // Sort so that content with the shortest lifespan are first.
        content = content.OrderBy(p => p.PromoteUntil).ToList();

        if (!string.IsNullOrEmpty(filterByTag))
        {
            content = content.Where(x => x.Tags.Contains(filterByTag, StringComparer.InvariantCultureIgnoreCase)).ToList();
        }

        if (itemCount == 0)
        {
            itemCount = content.Count;
        }

        AnsiConsole.MarkupLineInterpolated($"[yellow1]Total Posts:[/] {content.Count}");
        AnsiConsole.MarkupLineInterpolated($"[yellow1]Promoting first:[/] {itemCount}");
        AnsiConsole.WriteLine();

        return content.Take(itemCount);
    }
}