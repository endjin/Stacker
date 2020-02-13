// <copyright file="ServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Stacker.Cli.Commands;
    using Stacker.Cli.Commands.Environment;
    using Stacker.Cli.Commands.Environment.Init;
    using Stacker.Cli.Commands.Facebook;
    using Stacker.Cli.Commands.Facebook.Buffer;
    using Stacker.Cli.Commands.LinkedIn;
    using Stacker.Cli.Commands.LinkedIn.Buffer;
    using Stacker.Cli.Commands.Twitter;
    using Stacker.Cli.Commands.Twitter.Buffer;
    using Stacker.Cli.Commands.WordPress;
    using Stacker.Cli.Commands.WordPress.Export;
    using Stacker.Cli.Commands.WordPress.Export.LinkedIn;
    using Stacker.Cli.Commands.WordPress.Export.Twitter;
    using Stacker.Cli.Configuration;
    using Stacker.Cli.Configuration.Contracts;
    using Stacker.Cli.Domain.Buffer;

    public static class ServiceCollectionExtensions
    {
        public static void ConfigureDependencies(this ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAppEnvironment, FileSystemLocalProfileAppEnvironment>();
            serviceCollection.AddTransient<IStackerSettingsManager, StackerSettingsManager>();

            serviceCollection.AddTransient<ICommandFactory<WordPressCommandFactory>, WordPressCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<WordPressExportCommandFactory>, WordPressExportCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<WordPressExportLinkedInCommandFactory>, WordPressExportLinkedInCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<WordPressExportTwitterCommandFactory>, WordPressExportTwitterCommandFactory>();

            serviceCollection.AddTransient<ICommandFactory<EnvironmentCommandFactory>, EnvironmentCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<EnvironmentInitCommandFactory>, EnvironmentInitCommandFactory>();

            serviceCollection.AddTransient<ICommandFactory<FacebookCommandFactory>, FacebookCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<FacebookBufferCommandFactory>, FacebookBufferCommandFactory>();

            serviceCollection.AddTransient<ICommandFactory<LinkedInCommandFactory>, LinkedInCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<LinkedInBufferCommandFactory>, LinkedInBufferCommandFactory>();

            serviceCollection.AddTransient<ICommandFactory<TwitterCommandFactory>, TwitterCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<TwitterBufferCommandFactory>, TwitterBufferCommandFactory>();

            serviceCollection.AddTransient<IBufferClient, BufferClient>();

            serviceCollection.AddHttpClient();
        }
    }
}