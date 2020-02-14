// <copyright file="WordPressExportUniversalCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands.WordPress.Export.Universal
{
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
    using Stacker.Cli.Configuration.Contracts;
    using Stacker.Cli.Domain.Twitter;
    using Stacker.Cli.Domain.Universal;
    using Stacker.Cli.Domain.WordPress;

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

                    using (var reader = File.OpenText(wpexportFilePath))
                    {
                        var document = await XDocument.LoadAsync(reader, LoadOptions.None, CancellationToken.None).ConfigureAwait(false);
                        blogSite = new BlogSite(document);
                    }

                    Console.WriteLine($"Processing...");

                    var settings = this.settingsManager.LoadSettings(nameof(StackerSettings));
                    var posts = blogSite.GetAllPosts().ToList();
                    var validPosts = posts.FilterByValid(settings).ToList();
                    var promotablePosts = validPosts.FilterByPromotable().ToList();
                    var hashTagConverter = new WordPressToTwitterHashTagConverter();
                    var feed = new List<FeedItem>();

                    Console.WriteLine($"Total Posts: {posts.Count()}");
                    Console.WriteLine($"Valid Posts: {validPosts.Count()}");
                    Console.WriteLine($"Promotable Posts: {promotablePosts.Count()}");

                    // Sort the promotable posts so that those due to expire are at the top of the publication list.
                    foreach (var post in promotablePosts.OrderByDescending(p => p.PromoteUntil))
                    {
                        var user = settings.Users.Find(u => string.Equals(u.Email, post.Author.Email, StringComparison.InvariantCultureIgnoreCase));
                        var formatter = new TweetFormatter();

                        var tweet = new Tweet
                        {
                            AuthorHandle = user.Twitter,
                            AuthorDisplaName = post.Author.DisplayName,
                            Link = post.Link,
                            PublishedOn = post.PublishedAtUtc,
                            Title = post.Title,
                            Tags = post.Tags.Where(t => t != null).Select(t => hashTagConverter.Convert(t.Slug)),
                        };

                        if (post.MetaData.TryGetValue("og_desc", out string description))
                        {
                        }

                        feed.Add(new FeedItem
                        {
                            Author = new AuthorElement
                            {
                                DisplayName = post.Author.DisplayName,
                                Email = post.Author.Email,
                                TwitterHandle = user.Twitter,
                            },
                            Content = new Content
                            {
                              Editorial = description,
                              Tweet = formatter.Format(tweet),
                            },
                            PublishedOn = post.PublishedAtUtc,
                            PromoteUntil = post.PromoteUntil,
                            Tags = post.Tags.Where(t => t != null).Select(t => hashTagConverter.Convert(t.Slug)),
                        });
                    }

                    await using (var writer = File.CreateText(universalFilePath))
                    {
                        await writer.WriteAsync(JsonConvert.SerializeObject(feed)).ConfigureAwait(false);
                    }

                    Console.WriteLine($"Content written to {universalFilePath}");
                }),
            };

            cmd.AddArgument(new Argument<string>("wp-export-file-path") { Description = "WordPress Export file path." });
            cmd.AddArgument(new Argument<string>("universal-file-path") { Description = "Universal file path." });

            return cmd;
        }
    }
}