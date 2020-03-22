// <copyright file="ServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Stacker.Cli.Commands;
    using Stacker.Cli.Configuration;
    using Stacker.Cli.Contracts.Buffer;
    using Stacker.Cli.Contracts.Commands;
    using Stacker.Cli.Contracts.Configuration;
    using Stacker.Cli.Contracts.Tasks;
    using Stacker.Cli.Tasks;

    public static class ServiceCollectionExtensions
    {
        public static void ConfigureDependencies(this ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAppEnvironment, FileSystemLocalProfileAppEnvironment>();
            serviceCollection.AddTransient<IStackerSettingsManager, StackerSettingsManager>();

            serviceCollection.AddTransient<ICommandFactory<WordPressCommandFactory>, WordPressCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<WordPressExportCommandFactory>, WordPressExportCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<WordPressExportUniversalCommandFactory>, WordPressExportUniversalCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<WordPressExportMarkDownCommandFactory>, WordPressExportMarkDownCommandFactory>();

            serviceCollection.AddTransient<ICommandFactory<EnvironmentCommandFactory>, EnvironmentCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<EnvironmentInitCommandFactory>, EnvironmentInitCommandFactory>();

            serviceCollection.AddTransient<ICommandFactory<FacebookCommandFactory>, FacebookCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<FacebookBufferCommandFactory>, FacebookBufferCommandFactory>();

            serviceCollection.AddTransient<ICommandFactory<LinkedInCommandFactory>, LinkedInCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<LinkedInBufferCommandFactory>, LinkedInBufferCommandFactory>();

            serviceCollection.AddTransient<ICommandFactory<TwitterCommandFactory>, TwitterCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<TwitterBufferCommandFactory>, TwitterBufferCommandFactory>();

            serviceCollection.AddTransient<IBufferClient, BufferClient>();
            serviceCollection.AddTransient<IContentTasks, ContentTasks>();
            serviceCollection.AddTransient<IDownloadTasks, DownloadTasks>();

            serviceCollection.AddHttpClient();
        }
    }
}