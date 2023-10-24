// <copyright file="WordPressExportUniversalCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Newtonsoft.Json;
using Stacker.Cli.Configuration;
using Stacker.Cli.Contracts.Commands;
using Stacker.Cli.Contracts.Configuration;
using Stacker.Cli.Converters;
using Stacker.Cli.Domain.Universal;
using Stacker.Cli.Domain.WordPress;

namespace Stacker.Cli.Commands;

public class WordPressExportUniversalCommandFactory : ICommandFactory<WordPressExportUniversalCommandFactory>
{
    private readonly IStackerSettingsManager settingsManager;

    public WordPressExportUniversalCommandFactory(IStackerSettingsManager settingsManager)
    {
        this.settingsManager = settingsManager;
    }

    public Command Create()
    {
        var cmd = new Command("universal", "Convert WordPress export files into a universal format.")
        {
            Handler = CommandHandler.Create(async (string wpexportFilePath, string universalFilePath) =>
            {
                if (!File.Exists(wpexportFilePath))
                {
                    Console.WriteLine($"File not found {wpexportFilePath}");

                    return;
                }

                BlogSite blogSite;

                Console.WriteLine($"Reading {wpexportFilePath}");

                using (StreamReader reader = File.OpenText(wpexportFilePath))
                {
                    XDocument document = await XDocument.LoadAsync(reader, LoadOptions.None, CancellationToken.None).ConfigureAwait(false);
                    blogSite = new(document);
                }

                Console.WriteLine($"Processing...");

                StackerSettings settings = this.settingsManager.LoadSettings(nameof(StackerSettings));
                var posts = blogSite.GetAllPosts().ToList();
                var validPosts = posts.FilterByValid(settings).ToList();
                var promotablePosts = validPosts.FilterByPromotable().ToList();
                var hashTagConverter = new WordPressTagToHashTagConverter();
                var feed = new List<ContentItem>();

                Console.WriteLine($"Total Posts: {posts.Count()}");
                Console.WriteLine($"Valid Posts: {validPosts.Count()}");
                Console.WriteLine($"Promotable Posts: {promotablePosts.Count()}");

                foreach (Post post in promotablePosts)
                {
                    User user = settings.Users.Find(u => string.Equals(u.Email, post.Author.Email, StringComparison.InvariantCultureIgnoreCase));

                    feed.Add(new()
                    {
                        Author = new()
                        {
                            DisplayName = post.Author.DisplayName,
                            Email = post.Author.Email,
                            TwitterHandle = user.Twitter,
                        },
                        Content = new()
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
                        Tags = post.Tags.Where(t => t != null).Select(t => t.Name),
                    });
                }

                await using (StreamWriter writer = File.CreateText(universalFilePath))
                {
                    await writer.WriteAsync(JsonConvert.SerializeObject(feed, Formatting.Indented)).ConfigureAwait(false);
                }

                Console.WriteLine($"Content written to {universalFilePath}");
            }),
        };

        cmd.AddArgument(new Argument<string>("wp-export-file-path") { Description = "WordPress Export file path." });
        cmd.AddArgument(new Argument<string>("universal-file-path") { Description = "Universal file path." });

        return cmd;
    }
}