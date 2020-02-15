﻿// <copyright file="FacebookBufferCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands.Facebook.Buffer
{
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using Stacker.Cli.Configuration;
    using Stacker.Cli.Configuration.Contracts;
    using Stacker.Cli.Domain.Buffer;
    using Stacker.Cli.Domain.LinkedIn;
    using Stacker.Cli.Domain.Universal;

    public class FacebookBufferCommandFactory : ICommandFactory<FacebookBufferCommandFactory>
    {
        private readonly IStackerSettingsManager settingsManager;
        private readonly IBufferClient bufferClient;

        public FacebookBufferCommandFactory(IStackerSettingsManager settingsManager, IBufferClient bufferClient)
        {
            this.settingsManager = settingsManager;
            this.bufferClient = bufferClient;
        }

        public Command Create()
        {
            var cmd = new Command("buffer", "Uploads content to Buffer to be published via Facebook")
            {
                Handler = CommandHandler.Create(async (string contentFilePath, string profileName, int take) =>
                {
                    var settings = this.settingsManager.LoadSettings(nameof(StackerSettings));
                    var profileKey = $"facebook|{profileName}";

                    if (settings.BufferProfiles.ContainsKey(profileKey))
                    {
                        Console.WriteLine($"Loading: {contentFilePath}");

                        var content = JsonConvert.DeserializeObject<List<FeedItem>>(await File.ReadAllTextAsync(contentFilePath).ConfigureAwait(false)).OrderBy(p => p.PromoteUntil).ToList();
                        var profileId = settings.BufferProfiles[profileKey];

                        Console.WriteLine($"Buffer Profile: {profileKey} = {profileId}");

                        if (take == 0)
                        {
                            take = content.Count();
                        }

                        Console.WriteLine($"Total Posts: {content.Count()}");
                        Console.WriteLine($"Promoting first: {take}");

                        var formatter = new FacebookFormatter();
                        var postings = formatter.Format(content.Take(take));

                        await this.bufferClient.UploadAsync(postings, profileId).ConfigureAwait(false);
                    }
                    else
                    {
                        Console.WriteLine($"Settings for {profileName} not found. Please check your Stacker configuration.");
                    }
                }),
            };

            cmd.Add(new Argument<string>("content-file-path") { Description = "Content file path." });
            cmd.Add(new Argument<string>("profile-name") { Description = "Facebook profile to Buffer." });
            cmd.AddOption(new Option("--take") { Argument = new Argument<int>(), Description = "Number of posts to buffer. If omitted all content is buffered." });

            return cmd;
        }
    }
}