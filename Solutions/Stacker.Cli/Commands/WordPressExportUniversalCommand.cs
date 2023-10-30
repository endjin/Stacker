// <copyright file="WordPressExportUniversalCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.IO;

using Stacker.Cli.Configuration;
using Stacker.Cli.Contracts.Configuration;
using Stacker.Cli.Converters;
using Stacker.Cli.Domain.Universal;
using Stacker.Cli.Domain.WordPress;

namespace Stacker.Cli.Commands;

public class WordPressExportUniversalCommand : AsyncCommand<WordPressExportUniversalCommand.Settings>
{
    private readonly IStackerSettingsManager settingsManager;

    public WordPressExportUniversalCommand(IStackerSettingsManager settingsManager)
    {
        this.settingsManager = settingsManager;
    }

    /// <inheritdoc/>
    public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        if (!File.Exists(settings.WordPressExportFilePath.FullPath))
        {
            AnsiConsole.WriteLine($"File not found {settings.WordPressExportFilePath.FullPath}");

            return 1;
        }

        BlogSite blogSite;

        AnsiConsole.WriteLine($"Reading {settings.WordPressExportFilePath.FullPath}");

        using (StreamReader reader = File.OpenText(settings.WordPressExportFilePath.FullPath))
        {
            XDocument document = await XDocument.LoadAsync(reader, LoadOptions.None, CancellationToken.None).ConfigureAwait(false);
            blogSite = new BlogSite(document);
        }

        AnsiConsole.WriteLine($"Processing...");

        StackerSettings stackerSettings = this.settingsManager.LoadSettings(nameof(StackerSettings));
        List<Post> posts = blogSite.GetAllPosts().ToList();
        List<Post> validPosts = posts.FilterByValid(stackerSettings).ToList();
        List<Post> promotablePosts = validPosts.FilterByPromotable().ToList();
        TagToHashTagConverter hashTagConverter = new();
        List<ContentItem> feed = new();

        AnsiConsole.WriteLine($"Total Posts: {posts.Count()}");
        AnsiConsole.WriteLine($"Valid Posts: {validPosts.Count()}");
        AnsiConsole.WriteLine($"Promotable Posts: {promotablePosts.Count()}");

        foreach (Post post in promotablePosts)
        {
            User user = stackerSettings.Users.Find(u => string.Equals(u.Email, post.Author.Email, StringComparison.InvariantCultureIgnoreCase));

            feed.Add(new ContentItem
            {
                Author = new AuthorDetails
                {
                    DisplayName = post.Author.DisplayName,
                    Email = post.Author.Email,
                    TwitterHandle = user.Twitter,
                },
                Content = new ContentDetails
                {
                    Body = post.Body,
                    Excerpt = post.Excerpt,
                    Link = post.Link,
                    Title = post.Title,
                },
                PublishedOn = post.PublishedAtUtc,
                Promote = post.Promote,
                PromoteUntil = post.PromoteUntil,
                Status = post.Status,
                Slug = post.Slug,
                Tags = post.Tags.Where(t => t != null).Select(t => t.Name).ToList(),
            });
        }

        await using (StreamWriter writer = File.CreateText(settings.UniversalFilePath.FullPath))
        {
            JsonSerializerOptions options = new() { WriteIndented = true };
            await writer.WriteAsync(JsonSerializer.Serialize(feed, options)).ConfigureAwait(false);
        }

        AnsiConsole.WriteLine($"Content written to {settings.UniversalFilePath.FullPath}");

        return 0;
    }

    /// <summary>
    /// The settings for the command.
    /// </summary>
    public class Settings : CommandSettings
    {
        [CommandOption("-w|--wp-export-file-path <WordPressExportFilePath>")]
        [Description("WordPress Export file path.")]
        public FilePath WordPressExportFilePath { get; init; }

        [CommandOption("-o|--universal-file-path <UniversalFilePath>")]
        [Description("Universal file path.")]
        public FilePath UniversalFilePath { get; init; }
    }
}