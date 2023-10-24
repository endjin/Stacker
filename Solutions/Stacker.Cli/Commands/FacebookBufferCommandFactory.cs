// <copyright file="FacebookBufferCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using Stacker.Cli.Contracts.Commands;
using Stacker.Cli.Contracts.Tasks;
using Stacker.Cli.Domain.Publication;
using Stacker.Cli.Formatters;

namespace Stacker.Cli.Commands;

public class FacebookBufferCommandFactory : ICommandFactory<FacebookBufferCommandFactory>
{
    private readonly IContentTasks contentTasks;

    public FacebookBufferCommandFactory(IContentTasks contentTasks)
    {
        this.contentTasks = contentTasks;
    }

    public Command Create()
    {
        var cmd = new Command("buffer", "Uploads content to Buffer to be published via Facebook")
        {
            Handler = CommandHandler.Create(async (string contentFilePath, string profileName, int itemCount, DateTime fromDate, DateTime toDate, PublicationPeriod publicationPeriod) =>
                await this.contentTasks.BufferContentItemsAsync<FacebookFormatter>(contentFilePath, $"facebook|", profileName, publicationPeriod, fromDate, toDate, itemCount).ConfigureAwait(false)),
        };

        cmd.Add(new Argument<string>("content-file-path") { Description = "Content file path." });
        cmd.Add(new Argument<string>("profile-name") { Description = "Facebook profile to Buffer." });

        cmd.AddOption(new("--item-count",  "Number of content items to buffer. If omitted all content is buffered.") { Argument = new Argument<int>() });
        cmd.AddOption(new("--publication-period", "Publication period to filter content items by. If specified --from-date and --to-date are ignored.") { Argument = new Argument<PublicationPeriod>() });
        cmd.AddOption(new("--from-date", "Include content items published on, or after this date. Use YYYY/MM/DD Format. If omitted DateTime.MinValue is used.") { Argument = new Argument<DateTime>() });
        cmd.AddOption(new("--to-date", "Include content items published on, or before this date. Use YYYY/MM/DD Format. If omitted DateTime.MaxValue is used.") { Argument = new Argument<DateTime>() });

        return cmd;
    }
}