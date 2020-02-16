﻿// <copyright file="WordPressExportCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands
{
    using System.CommandLine;
    using Stacker.Cli.Contracts.Commands;

    public class WordPressExportCommandFactory : ICommandFactory<WordPressExportCommandFactory>
    {
        private readonly ICommandFactory<WordPressExportUniversalCommandFactory> universalExportCommandFactory;

        public WordPressExportCommandFactory(ICommandFactory<WordPressExportUniversalCommandFactory> universalExportCommandFactory)
        {
            this.universalExportCommandFactory = universalExportCommandFactory;
        }

        public Command Create()
        {
            var cmd = new Command("export", "Perform operations on WordPress export files.");
            cmd.AddCommand(this.universalExportCommandFactory.Create());

            return cmd;
        }
    }
}