// <copyright file="LinkedInBufferCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands.LinkedIn.Buffer
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.IO;
    using System.Linq;
    using Stacker.Cli.Configuration;
    using Stacker.Cli.Configuration.Contracts;
    using Stacker.Cli.Domain.Buffer;

    public class LinkedInBufferCommandFactory : ICommandFactory<LinkedInBufferCommandFactory>
    {
        private readonly IStackerSettingsManager settingsManager;
        private readonly IBufferClient bufferClient;

        public LinkedInBufferCommandFactory(IStackerSettingsManager settingsManager, IBufferClient bufferClient)
        {
            this.settingsManager = settingsManager;
            this.bufferClient = bufferClient;
        }

        public Command Create()
        {
            var cmd = new Command("buffer", "Uploads content to Buffer to be published via Twitter")
            {
                Handler = CommandHandler.Create(async (string linkedInFilePath, string profileName, int take) =>
                {
                    var settings = this.settingsManager.LoadSettings(nameof(StackerSettings));
                    var profileKey = $"linkedin|{profileName}";

                    if (settings.BufferProfiles.ContainsKey(profileKey))
                    {
                        var profileId = settings.BufferProfiles[profileKey];

                        if (take == 0)
                        {
                            take = int.MaxValue;
                        }

                        await this.bufferClient.UploadAsync((await File.ReadAllLinesAsync(linkedInFilePath).ConfigureAwait(false)).Take(take), profileId).ConfigureAwait(false);
                    }
                    else
                    {
                        Console.WriteLine($"Settings for {profileName} not found. Please check your Stacker configuration.");
                    }
                }),
            };

            cmd.Add(new Argument<string>("linkedin-file-path") { Description = "LinkedIn file path." });
            cmd.Add(new Argument<string>("profile-name") { Description = "LinkedIn profile to Buffer." });
            cmd.AddOption(new Option("--take") { Argument = new Argument<int>(), Description = "Number of posts to buffer. If omitted all content is buffered." });

            /*cmd.AddOption(new Option("--from-date") { Argument = new Argument<DateTime> { Description = "Number of Tweets to buffer" } });
            cmd.AddOption(new Option("--to-date") { Argument = new Argument<DateTime> { Description = "Number of Tweets to buffer" } });*/

            return cmd;
        }
    }
}