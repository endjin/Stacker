// <copyright file="ServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Stacker.Cli.Cleaners;
using Stacker.Cli.Configuration;
using Stacker.Cli.Contracts.Buffer;
using Stacker.Cli.Contracts.Configuration;
using Stacker.Cli.Contracts.Tasks;
using Stacker.Cli.Serialization;
using Stacker.Cli.Tasks;
using Environment = System.Environment;
using Path = System.IO.Path;

namespace Stacker.Cli.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureDependencies(this ServiceCollection serviceCollection)
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "endjin", "stacker", "configuration"))
            .AddJsonFile("StackerSettings.json", optional: true, reloadOnChange: false);

        string? settingsPath = Environment.GetEnvironmentVariable("STACKER_SETTINGS_PATH");
        if (!string.IsNullOrEmpty(settingsPath))
        {
            builder.AddJsonFile(settingsPath, optional: false);
        }

        string? settingsJson = Environment.GetEnvironmentVariable("STACKER_SETTINGS_JSON");
        if (!string.IsNullOrEmpty(settingsJson))
        {
            builder.AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(settingsJson)));
        }

        builder.AddEnvironmentVariables();

        IConfigurationRoot configurationRoot = builder.Build();

        StackerSettings options = new()
        {
            BufferAccessToken = string.Empty,
            WordPressToMarkdown = new WordPressToMarkdown(),
        };
        configurationRoot.Bind(options);

        serviceCollection.AddSingleton(options);

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