// <copyright file="WordPressExportTwitterCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands.WordPress.Export.Twitter
{
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Xml.Linq;
    using Stacker.Cli.Configuration;
    using Stacker.Cli.Configuration.Contracts;
    using Stacker.Cli.Domain.Twitter;
    using Stacker.Cli.Domain.WordPress;

    public class WordPressExportTwitterCommandFactory : ICommandFactory<WordPressExportTwitterCommandFactory>
    {
        private readonly IStackerSettingsManager settingsManager;

        public WordPressExportTwitterCommandFactory(IStackerSettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        public Command Create()
        {
            var cmd = new Command("twitter", "Convert WordPress export files for publication in Twitter")
            {
                Handler = CommandHandler.Create(async (string wpexportfilepath, string twitterfilepath) =>
                {
                    if (!File.Exists(wpexportfilepath))
                    {
                        Console.WriteLine($"File not found {wpexportfilepath}");

                        return;
                    }

                    BlogSite blogSite;

                    using (var reader = File.OpenText(wpexportfilepath))
                    {
                        var document = await XDocument.LoadAsync(reader, LoadOptions.None, CancellationToken.None).ConfigureAwait(false);
                        blogSite = new BlogSite(document);
                    }

                    var settings = this.settingsManager.LoadSettings(nameof(StackerSettings));
                    var posts = blogSite.GetAllPosts().ToList();
                    var validPosts = posts.FilterByValid(settings).ToList();
                    var promotablePosts = validPosts.FilterByPromotable().ToList();
                    var tweets = new List<Tweet>();
                    var hashTagConverter = new WordPressToTwitterHashTagConverter();

                    Console.WriteLine($"Total Posts: {posts.Count()}");
                    Console.WriteLine($"Valid Posts: {validPosts.Count()}");
                    Console.WriteLine($"Promotable Posts: {promotablePosts.Count()}");

                    // Sort the promotable posts so that those due to expire are at the top of the publication list.
                    foreach (var post in promotablePosts.OrderByDescending(p => p.PromoteUntil))
                    {
                        var user = settings.Users.Find(u => string.Equals(u.Email, post.Author.Email, StringComparison.InvariantCultureIgnoreCase));

                        var tweet = new Tweet
                        {
                            AuthorHandle = user.Twitter,
                            AuthorDisplaName = post.Author.DisplayName,
                            Link = post.Link,
                            PublishedOn = post.PublishedAtUtc,
                            Title = post.Title,
                            Tags = post.Tags.Where(t => t != null).Select(t => hashTagConverter.Convert(t.Slug)),
                        };

                        tweets.Add(tweet);
                    }

                    var formatter = new TweetFormatter();

                    await using (var writer = File.CreateText(twitterfilepath))
                    {
                        foreach (var tweet in tweets)
                        {
                            await writer.WriteLineAsync(formatter.Format(tweet)).ConfigureAwait(false);
                        }
                    }
                }),
            };

            cmd.AddArgument(new Argument<string>("--wp-export-file-path") { Description = "WordPress Export file path." });
            cmd.AddArgument(new Argument<string>("--twitter-file-path") { Description = "Twitter file path." });

            return cmd;
        }
    }
}