// <copyright file="WordPressExportMarkDownCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands
{
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Xml.Linq;
    using ReverseMarkdown;
    using Stacker.Cli.Configuration;
    using Stacker.Cli.Contracts.Commands;
    using Stacker.Cli.Contracts.Configuration;
    using Stacker.Cli.Converters;
    using Stacker.Cli.Domain.Universal;
    using Stacker.Cli.Domain.WordPress;

    public class WordPressExportMarkDownCommandFactory : ICommandFactory<WordPressExportMarkDownCommandFactory>
    {
        private readonly IStackerSettingsManager settingsManager;

        public WordPressExportMarkDownCommandFactory(IStackerSettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        public Command Create()
        {
            var cmd = new Command("markdown", "Convert WordPress export files into a markdown format.")
            {
                Handler = CommandHandler.Create(async (string wpexportFilePath, string exportFilePath) =>
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

                    var converter = new Converter(new Config
                    {
                        UnknownTags = Config.UnknownTagsOption.PassThrough, // Include the unknown tag completely in the result (default as well)
                        RemoveComments = true, // will ignore all comments
                        SmartHrefHandling = true, // remove markdown output for links where appropriate
                    });

                    var settings = this.settingsManager.LoadSettings(nameof(StackerSettings));
                    var posts = blogSite.GetAllPosts().ToList();
                    var feed = new List<ContentItem>();

                    Console.WriteLine($"Total Posts: {posts.Count()}");

                    foreach (var post in posts)
                    {
                        var user = settings.Users.Find(u => string.Equals(u.Email, post.Author.Email, StringComparison.InvariantCultureIgnoreCase));

                        feed.Add(new ContentItem
                        {
                            Author = new AuthorDetails
                            {
                                DisplayName = post.Author.DisplayName,
                                Email = post.Author.Email,
                                TwitterHandle = user.Twitter,
                            },
                            Categories = post.Categories.Select(c => c.Name),
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
                            Slug = post.Slug,
                            Status = post.Status,
                            Tags = post.Tags.Where(t => t != null).Select(t => t.Name),
                        });
                    }

                    var sb = new StringBuilder();
                    FileInfo fi = new FileInfo(exportFilePath);

                    if (!fi.Directory.Exists)
                    {
                        fi.Directory.Create();
                    }

                    foreach (var ci in feed)
                    {
                        sb.AppendLine("---");
                        sb.Append("Title: ");
                        sb.Append(ci.Content.Title);
                        sb.Append(Environment.NewLine);
                        sb.Append("Date: ");
                        sb.Append(ci.PublishedOn);
                        sb.Append(Environment.NewLine);
                        sb.Append("Author: ");
                        sb.Append(ci.Author.DisplayName);
                        sb.Append(Environment.NewLine);
                        sb.Append("Category: [");
                        sb.Append(string.Join(",", ci.Categories));
                        sb.Append("]");
                        sb.Append(Environment.NewLine);
                        sb.Append("Tags: [");
                        sb.Append(string.Join(",", ci.Tags));
                        sb.Append("]");
                        sb.Append(Environment.NewLine);
                        sb.Append("Slug: ");
                        sb.Append(ci.Slug);
                        sb.Append(Environment.NewLine);
                        sb.Append("Status: ");
                        sb.Append(ci.Status);
                        sb.Append(Environment.NewLine);
                        sb.Append("Attachments: ");
                        sb.Append(string.Empty);
                        sb.Append(Environment.NewLine);
                        sb.AppendLine("---");
                        sb.Append(Environment.NewLine);
                        sb.Append(ci.Content.Body);

                        await using (var writer = File.CreateText(Path.Combine(exportFilePath, ci.Slug + ".md")))
                        {
                            await writer.WriteAsync(sb.ToString()).ConfigureAwait(false);
                        }

                        sb.Clear();
                    }

                    Console.WriteLine($"Content written to {exportFilePath}");
                }),
            };

            cmd.AddArgument(new Argument<string>("wp-export-file-path") { Description = "WordPress Export file path." });
            cmd.AddArgument(new Argument<string>("export-file-path") { Description = "File path for the exported files." });

            return cmd;
        }
    }
}