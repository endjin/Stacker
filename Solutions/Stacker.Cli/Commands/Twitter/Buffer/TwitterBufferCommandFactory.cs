// <copyright file="TwitterBufferCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands.Twitter.Buffer
{
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using NodaTime;
    using Stacker.Cli.Configuration;
    using Stacker.Cli.Configuration.Contracts;
    using Stacker.Cli.Domain.Buffer;
    using Stacker.Cli.Domain.Twitter;
    using Stacker.Cli.Domain.Universal;

    public class TwitterBufferCommandFactory : ICommandFactory<TwitterBufferCommandFactory>
    {
        private readonly IStackerSettingsManager settingsManager;
        private readonly IBufferClient bufferClient;

        public TwitterBufferCommandFactory(IStackerSettingsManager settingsManager, IBufferClient bufferClient)
        {
            this.settingsManager = settingsManager;
            this.bufferClient = bufferClient;
        }

        public Command Create()
        {
            var cmd = new Command("buffer", "Uploads content to Buffer to be published via Twitter")
            {
                Handler = CommandHandler.Create(async (string contentFilePath, string profileName, int take, DateTime fromDate, DateTime toDate, TimePeriod timePeriod) =>
                {
                    var settings = this.settingsManager.LoadSettings(nameof(StackerSettings));
                    var profileKey = $"twitter|{profileName}";

                    if (settings.BufferProfiles.ContainsKey(profileKey))
                    {
                        Console.WriteLine($"Loading: {contentFilePath}");

                        var content = JsonConvert.DeserializeObject<IEnumerable<FeedItem>>(await File.ReadAllTextAsync(contentFilePath).ConfigureAwait(false));

                        if (timePeriod != TimePeriod.None)
                        {
                            var dateRange = new TimePeriodConverter().Convert(timePeriod);

                            content = content.Where(p => (LocalDate.FromDateTime(p.PublishedOn.LocalDateTime) > dateRange.Start) && (LocalDate.FromDateTime(p.PublishedOn.LocalDateTime) < dateRange.End));
                        }

                        if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                        {
                            content = content.Where(p => p.PublishedOn.LocalDateTime > fromDate && p.PublishedOn.LocalDateTime < toDate);
                        }

                        content = content.OrderBy(p => p.PromoteUntil).ToList();

                        var profileId = settings.BufferProfiles[profileKey];

                        Console.WriteLine($"Buffer Profile: {profileKey} = {profileId}");

                        if (take == 0)
                        {
                            take = content.Count();
                        }

                        Console.WriteLine($"Total Posts: {content.Count()}");
                        Console.WriteLine($"Promoting first: {take}");

                        var formatter = new TweetFormatter();
                        var tweets = formatter.Format(content.Take(take));

                        await this.bufferClient.UploadAsync(tweets, profileId).ConfigureAwait(false);
                    }
                    else
                    {
                        Console.WriteLine($"Settings for {profileName} not found. Please check your Stacker configuration.");
                    }
                }),
            };

            cmd.Add(new Argument<string>("content-file-path") { Description = "Content file path." });
            cmd.Add(new Argument<string>("profile-name") { Description = "Twitter profile to Buffer." });

            cmd.AddOption(new Option("--take") { Argument = new Argument<int>(), Description = "Number of posts to buffer. If omitted all content is buffered." });
            cmd.AddOption(new Option("--from-date") { Argument = new Argument<DateTime> { Description = "Number of Tweets to buffer" } });
            cmd.AddOption(new Option("--to-date") { Argument = new Argument<DateTime> { Description = "Number of Tweets to buffer" } });
            cmd.AddOption(new Option("--time-period") { Argument = new Argument<TimePeriod> { Description = "Time period to select content." } });

            return cmd;
        }
    }
}