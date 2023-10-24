// <copyright file="ServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;

using Stacker.Cli.Cleaners;
using Stacker.Cli.Configuration;
using Stacker.Cli.Contracts.Buffer;
using Stacker.Cli.Contracts.Configuration;
using Stacker.Cli.Contracts.Tasks;
using Stacker.Cli.Serialization;
using Stacker.Cli.Tasks;

namespace Stacker.Cli.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureDependencies(this ServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IAppEnvironment, FileSystemLocalProfileAppEnvironment>();
        serviceCollection.AddTransient<IStackerSettingsManager, StackerSettingsManager>();

        serviceCollection.AddTransient<IBufferClient, BufferClient>();
        serviceCollection.AddTransient<IContentTasks, ContentTasks>();
        serviceCollection.AddTransient<IDownloadTasks, DownloadTasks>();
        serviceCollection.AddTransient<IYamlSerializerFactory, YamlSerializerFactory>();

        serviceCollection.AddTransient<ContentItemCleaner>();
        serviceCollection.AddTransient<IPreDownloadCleaner, ContentItemAttachmentPathCleaner>();
        serviceCollection.AddTransient<IPreDownloadCleaner, WordPressImageResizerCleaner>();
        serviceCollection.AddTransient<IPreDownloadCleaner, ReplaceNewLineWithParagraphTagCleaner>();
        serviceCollection.AddTransient<IPreDownloadCleaner, EnsureEndjinHttpsInBody>();
        serviceCollection.AddTransient<IPostDownloadCleaner, RemoveHostNamesFromBody>();
        serviceCollection.AddTransient<IPostConvertCleaner, RemoveHeaderImageFromBody>();
        serviceCollection.AddTransient<IPostConvertCleaner, RemoveThreeBlankLinesFromStartBody>();
        serviceCollection.AddTransient<IPostConvertCleaner, ReplaceWpUploadPath>();
        serviceCollection.AddTransient<IPostConvertCleaner, UpdateInternalPostUrls>();
        serviceCollection.AddTransient<IPostConvertCleaner, ReplaceSmartQuotes>();

        serviceCollection.AddHttpClient();
    }
}