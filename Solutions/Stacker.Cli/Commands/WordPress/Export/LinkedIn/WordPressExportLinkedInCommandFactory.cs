// <copyright file="WordPressExportLinkedInCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands.WordPress.Export.LinkedIn
{
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using Stacker.Cli.Configuration.Contracts;

    public class WordPressExportLinkedInCommandFactory : ICommandFactory<WordPressExportLinkedInCommandFactory>
    {
        private readonly IAppEnvironment appEnvironment;

        public WordPressExportLinkedInCommandFactory(IAppEnvironment appEnvironment)
        {
            this.appEnvironment = appEnvironment;
        }

        public Command Create()
        {
            return new Command("linkedin", "Convert WordPress export files for publication in LinkedIn")
            {
                Handler = CommandHandler.Create(() => this.appEnvironment.Initialize()),
            };
        }
    }
}