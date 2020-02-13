// <copyright file="WordPressExportLinkedInCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands.WordPress.Export.LinkedIn
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
    using Stacker.Cli.Domain.LinkedIn;
    using Stacker.Cli.Domain.Twitter;
    using Stacker.Cli.Domain.WordPress;

    public class WordPressExportLinkedInCommandFactory : ICommandFactory<WordPressExportLinkedInCommandFactory>
    {
        private readonly IStackerSettingsManager settingsManager;

        public WordPressExportLinkedInCommandFactory(IStackerSettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        public Command Create()
        {
            var cmd = new Command("linkedin", "Convert WordPress export files for publication in LinkedIn")
            {
                Handler = CommandHandler.Create(async (string wpexportfilepath, string linkedinfilepath) =>
                {
                    if (!File.Exists(wpexportfilepath))
                    {
                        Console.WriteLine($"File not found {wpexportfilepath}");

                        return;
                    }

                    BlogSite blogSite;

                    Console.WriteLine($"Reading {wpexportfilepath}");

                    using (var reader = File.OpenText(wpexportfilepath))
                    {
                        var document = await XDocument.LoadAsync(reader, LoadOptions.None, CancellationToken.None).ConfigureAwait(false);
                        blogSite = new BlogSite(document);
                    }

                    Console.WriteLine($"Processing...");

                    var settings = this.settingsManager.LoadSettings(nameof(StackerSettings));
                    var posts = blogSite.GetAllPosts().ToList();
                    var validPosts = posts.FilterByValid(settings).ToList();
                    var promotablePosts = validPosts.FilterByPromotable().ToList();
                    var postings = new List<Posting>();
                    var hashTagConverter = new WordPressToTwitterHashTagConverter();

                    Console.WriteLine($"Total Posts: {posts.Count()}");
                    Console.WriteLine($"Valid Posts: {validPosts.Count()}");
                    Console.WriteLine($"Promotable Posts: {promotablePosts.Count()}");

                    foreach (var post in promotablePosts.OrderByDescending(p => p.PromoteUntil))
                    {
                        if (post.MetaData.TryGetValue("og_desc", out string description))
                        {
                        }

                        postings.Add(new Posting
                        {
                            Author = post.Author.DisplayName,
                            Body = description,
                            Image = new Image { Title = post.FeaturedImage?.Title, Url = post.FeaturedImage?.Url },
                            Link = post.Link,
                            Tags = post.Tags.Where(t => t != null).Select(t => hashTagConverter.Convert(t.Slug)),
                        });
                    }

                    var formatter = new LinkedInFormatter();
                    var renderedPosts = new List<RenderedPosting>();

                    foreach (var posting in postings)
                    {
                        renderedPosts.Add(new RenderedPosting
                        {
                            Content = formatter.Format(posting),
                        });
                    }

                    await using (var writer = File.CreateText(linkedinfilepath))
                    {
                        await writer.WriteAsync(JsonConvert.SerializeObject(renderedPosts)).ConfigureAwait(false);
                    }

                    Console.WriteLine($"Content written to {linkedinfilepath}");
                }),
            };

            cmd.AddArgument(new Argument<string>("--wp-export-file-path") { Description = "WordPress Export file path." });
            cmd.AddArgument(new Argument<string>("--linkedin-file-path") { Description = "LinkedIn file path." });

            return cmd;
        }
    }
}