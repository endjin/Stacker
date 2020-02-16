// <copyright file="LinkedInBufferCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using Stacker.Cli.Contracts.Commands;
    using Stacker.Cli.Contracts.Tasks;
    using Stacker.Cli.Domain.Publication;
    using Stacker.Cli.Formatters;

    public class LinkedInBufferCommandFactory : ICommandFactory<LinkedInBufferCommandFactory>
    {
        private readonly IContentTasks contentTasks;

        public LinkedInBufferCommandFactory(IContentTasks contentTasks)
        {
            this.contentTasks = contentTasks;
        }

        public Command Create()
        {
            var cmd = new Command("buffer", "Uploads content to Buffer to be published via Twitter")
            {
                Handler = CommandHandler.Create(async (string contentFilePath, string profileName, int itemCount, DateTime fromDate, DateTime toDate, PublicationPeriod publicationPeriod) =>
                {
                    await this.contentTasks.BufferContentItemsAsync<LinkedInFormatter>(contentFilePath, $"linkedin|{profileName}", publicationPeriod, fromDate, toDate, itemCount).ConfigureAwait(false);
                }),
            };

            cmd.Add(new Argument<string>("content-file-path") { Description = "Content file path." });
            cmd.Add(new Argument<string>("profile-name") { Description = "LinkedIn profile to Buffer." });

            cmd.AddOption(new Option("--item-count") { Argument = new Argument<int>(), Description = "Number of posts to buffer. If omitted all content is buffered." });
            cmd.AddOption(new Option("--from-date") { Argument = new Argument<DateTime> { Description = "Number of Tweets to buffer" } });
            cmd.AddOption(new Option("--to-date") { Argument = new Argument<DateTime> { Description = "Number of Tweets to buffer" } });
            cmd.AddOption(new Option("--time-period") { Argument = new Argument<PublicationPeriod> { Description = "Time period to select content." } });

            return cmd;
        }
    }
}