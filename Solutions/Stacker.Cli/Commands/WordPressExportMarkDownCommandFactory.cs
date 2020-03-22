// <copyright file="WordPressExportMarkDownCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands
{
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Xml.Linq;
    using ReverseMarkdown;
    using Stacker.Cli.Configuration;
    using Stacker.Cli.Contracts.Commands;
    using Stacker.Cli.Contracts.Configuration;
    using Stacker.Cli.Domain.Universal;
    using Stacker.Cli.Domain.WordPress;
    using Stacker.Cli.Tasks;

    public class WordPressExportMarkDownCommandFactory : ICommandFactory<WordPressExportMarkDownCommandFactory>
    {
        private readonly IDownloadTasks downloadTasks;
        private readonly IStackerSettingsManager settingsManager;

        public WordPressExportMarkDownCommandFactory(IStackerSettingsManager settingsManager, IDownloadTasks downloadTasks)
        {
            this.settingsManager = settingsManager;
            this.downloadTasks = downloadTasks;
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
                    var posts = blogSite.GetAllPostsInAllPublicationStates().ToList();
                    var feed = new List<ContentItem>();

                    Console.WriteLine($"Total Posts: {posts.Count()}");

                    var attachments = posts.Where(x => x.Attachments.Any());

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
                                Username = post.Author.Username,
                            },
                            Categories = post.Categories.Select(c => c.Name),
                            Content = new ContentDetails
                            {
                                Attachments = post.Attachments.Select(x => new ContentAttachment { Path = "/content/images/blog/" + x.Path, Url = x.Url }),
                                Body = post.Body.Replace("\n", "<p/>").Replace("https://blogs.endjin.com/wp-content/uploads", "/content/images/blog"),
                                Excerpt = post.Excerpt,
                                Link = post.Link,
                                Title = post.Title,
                            },
                            Id = post.Id,
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
                    DirectoryInfo tempHtmlFolder = new DirectoryInfo(Path.Join(Path.GetTempPath(), "stacker", "html"));
                    DirectoryInfo tempMarkdownFolder = new DirectoryInfo(Path.Join(Path.GetTempPath(), "stacker", "md"));
                    string inputTempHtmlFilePath;
                    string outputTempMarkdownFilePath;
                    string outputFilePath;

                    if (!fi.Directory.Exists)
                    {
                        fi.Directory.Create();
                    }

                    if (!tempHtmlFolder.Exists)
                    {
                        tempHtmlFolder.Create();
                    }

                    if (!tempMarkdownFolder.Exists)
                    {
                        tempMarkdownFolder.Create();
                    }

                    await this.downloadTasks.DownloadAsync(feed, exportFilePath).ConfigureAwait(false);

                    foreach (var ci in feed)
                    {
                        sb.AppendLine("---");
                        sb.Append("Title: ");
                        sb.Append(ci.Content.Title);
                        sb.Append(Environment.NewLine);
                        sb.Append("Date: ");
                        sb.Append(ci.PublishedOn.ToString("O"));
                        sb.Append(Environment.NewLine);
                        sb.Append("Author: ");
                        sb.Append(ci.Author.Username);
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

                        if (string.IsNullOrEmpty(ci.Slug))
                        {
                            ci.Slug = new string(ci.Content.Title.ToLowerInvariant().Replace(" ", "-").Replace("---", "-").Replace("--", "-").Where(ch => !Path.GetInvalidFileNameChars().Contains(ch)).ToArray());
                        }

                        sb.Append(ci.Slug);
                        sb.Append(Environment.NewLine);
                        sb.Append("Status: ");
                        sb.Append(ci.Status);
                        sb.Append(Environment.NewLine);
                        sb.Append("Attachments: ");
                        sb.Append(string.Join(",", ci.Content.Attachments.Select(x => x.Path).OrderBy(x => x)));
                        sb.Append(Environment.NewLine);
                        sb.AppendLine("---");
                        sb.Append(Environment.NewLine);

                        await using (var writer = File.CreateText(Path.Combine(tempHtmlFolder.FullName, ci.UniqueId + ".html")))
                        {
                            await writer.WriteAsync(ci.Content.Body).ConfigureAwait(false);
                        }

                        inputTempHtmlFilePath = Path.Combine(tempHtmlFolder.FullName, ci.UniqueId + ".html");
                        outputTempMarkdownFilePath = Path.Combine(tempMarkdownFolder.FullName, ci.UniqueId + ".md");
                        outputFilePath = Path.Combine(exportFilePath, ci.UniqueId + ".md");

                        if (ExecutePandoc(inputTempHtmlFilePath, outputTempMarkdownFilePath))
                        {
                            sb.Append(await File.ReadAllTextAsync(outputTempMarkdownFilePath).ConfigureAwait(false));

                            await using (var writer = File.CreateText(outputFilePath))
                            {
                                await writer.WriteAsync(sb.ToString()).ConfigureAwait(false);
                            }

                            Console.WriteLine(outputFilePath);
                        }

                        // Remote the temporary html file.
                        File.Delete(inputTempHtmlFilePath);

                        sb.Clear();
                    }
                }),
            };

            cmd.AddArgument(new Argument<string>("wp-export-file-path") { Description = "WordPress Export file path." });
            cmd.AddArgument(new Argument<string>("export-file-path") { Description = "File path for the exported files." });

            return cmd;
        }

        private static bool ExecutePandoc(string inputTempHtmlFilePath, string outputTempMarkdownFilePath)
        {
            bool success = false;

            string arguments = $"-f html+raw_html --to=markdown-smart-raw_html --wrap=preserve -o \"{outputTempMarkdownFilePath}\" \"{inputTempHtmlFilePath}\" ";

            var psi = new ProcessStartInfo
            {
                FileName = "pandoc",
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
            };

            var process = new System.Diagnostics.Process { StartInfo = psi };
            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                Console.WriteLine("Failed to convert " + outputTempMarkdownFilePath);
                Console.WriteLine(process.StandardError.ReadToEnd());
            }
            else
            {
                success = true;
            }

            return success;
        }
    }
}