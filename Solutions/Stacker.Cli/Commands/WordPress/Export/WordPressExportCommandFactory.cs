// <copyright file="WordPressExportCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands.WordPress.Export
{
    using System.CommandLine;
    using Stacker.Cli.Commands.WordPress.Export.LinkedIn;
    using Stacker.Cli.Commands.WordPress.Export.Twitter;
    using Stacker.Cli.Configuration.Contracts;

    public class WordPressExportCommandFactory : ICommandFactory<WordPressExportCommandFactory>
    {
        private readonly IAppEnvironment appEnvironment;
        private readonly ICommandFactory<WordPressExportLinkedInCommandFactory> wordpressExportLinkedInCommandFactory;
        private readonly ICommandFactory<WordPressExportTwitterCommandFactory> wordpressExportTwitterCommandFactory;

        public WordPressExportCommandFactory(
            IAppEnvironment appEnvironment,
            ICommandFactory<WordPressExportLinkedInCommandFactory> wordpressExportLinkedInCommandFactory,
            ICommandFactory<WordPressExportTwitterCommandFactory> wordpressExportTwitterCommandFactory)
        {
            this.appEnvironment = appEnvironment;
            this.wordpressExportLinkedInCommandFactory = wordpressExportLinkedInCommandFactory;
            this.wordpressExportTwitterCommandFactory = wordpressExportTwitterCommandFactory;
        }

        public Command Create()
        {
            var cmd = new Command("export", "Perform operations on WordPress export files.");
            cmd.AddCommand(this.wordpressExportLinkedInCommandFactory.Create());
            cmd.AddCommand(this.wordpressExportTwitterCommandFactory.Create());

            return cmd;
        }
    }
}