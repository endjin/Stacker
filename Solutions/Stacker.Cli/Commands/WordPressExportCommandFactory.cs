// <copyright file="WordPressExportCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.CommandLine;
using Stacker.Cli.Contracts.Commands;

namespace Stacker.Cli.Commands;

public class WordPressExportCommandFactory : ICommandFactory<WordPressExportCommandFactory>
{
    private readonly ICommandFactory<WordPressExportUniversalCommandFactory> universalExportCommandFactory;
    private readonly ICommandFactory<WordPressExportMarkDownCommandFactory> markdownExportCommandFactory;

    public WordPressExportCommandFactory(
        ICommandFactory<WordPressExportUniversalCommandFactory> universalExportCommandFactory,
        ICommandFactory<WordPressExportMarkDownCommandFactory> markdownExportCommandFactory)
    {
        this.universalExportCommandFactory = universalExportCommandFactory;
        this.markdownExportCommandFactory = markdownExportCommandFactory;
    }

    public Command Create()
    {
        var cmd = new Command("export", "Perform operations on WordPress export files.");
        cmd.AddCommand(this.universalExportCommandFactory.Create());
        cmd.AddCommand(this.markdownExportCommandFactory.Create());

        return cmd;
    }
}