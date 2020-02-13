// <copyright file="WordPressCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands.WordPress
{
    using System.CommandLine;
    using Stacker.Cli.Commands.WordPress.Export;

    public class WordPressCommandFactory : ICommandFactory<WordPressCommandFactory>
    {
        private readonly ICommandFactory<WordPressExportCommandFactory> wordpressExportCommandFactory;

        public WordPressCommandFactory(ICommandFactory<WordPressExportCommandFactory> wordpressExportCommandFactory)
        {
            this.wordpressExportCommandFactory = wordpressExportCommandFactory;
        }

        public Command Create()
        {
            var cmd = new Command("wordpress", "WordPress functionality.");

            cmd.AddCommand(this.wordpressExportCommandFactory.Create());

            return cmd;
        }
    }
}