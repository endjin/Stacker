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
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Xml.Linq;
    using Flurl;
    using ReverseMarkdown;
    using Stacker.Cli.Cleaners;
    using Stacker.Cli.Configuration;
    using Stacker.Cli.Contracts.Commands;
    using Stacker.Cli.Contracts.Configuration;
    using Stacker.Cli.Domain.Universal;
    using Stacker.Cli.Domain.WordPress;
    using Stacker.Cli.Serialization;
    using Stacker.Cli.Tasks;
    using YamlDotNet.Serialization;

    public class WordPressExportMarkDownCommandFactory : ICommandFactory<WordPressExportMarkDownCommandFactory>
    {
        private readonly IDownloadTasks downloadTasks;
        private readonly IStackerSettingsManager settingsManager;
        private readonly ContentItemCleaner cleanerManager;
        private readonly IYamlSerializerFactory serializerFactory;
        private ISerializer serializer;

        public WordPressExportMarkDownCommandFactory(IStackerSettingsManager settingsManager, IDownloadTasks downloadTasks, ContentItemCleaner cleanerManager, IYamlSerializerFactory serializerFactory)
        {
            this.settingsManager = settingsManager;
            this.downloadTasks = downloadTasks;
            this.cleanerManager = cleanerManager;
            this.serializerFactory = serializerFactory;
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

                    this.serializer = this.serializerFactory.GetSerializer();

                    BlogSite blogSite = await this.LoadWordPressExportAsync(wpexportFilePath).ConfigureAwait(false);

                    var feed = this.LoadFeed(blogSite);

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

                    // await this.downloadTasks.DownloadAsync(feed, exportFilePath).ConfigureAwait(false);
                    foreach (var ci in feed)
                    {
                        var contentItem = this.cleanerManager.PostDownload(ci);

                        sb.AppendLine("---");
                        sb.Append(this.CreateYamlHeader(contentItem));
                        sb.Append("---");
                        sb.Append(Environment.NewLine);
                        sb.Append(Environment.NewLine);

                        await using (var writer = File.CreateText(Path.Combine(tempHtmlFolder.FullName, contentItem.UniqueId + ".html")))
                        {
                            await writer.WriteAsync(contentItem.Content.Body).ConfigureAwait(false);
                        }

                        inputTempHtmlFilePath = Path.Combine(tempHtmlFolder.FullName, contentItem.UniqueId + ".html");
                        outputTempMarkdownFilePath = Path.Combine(tempMarkdownFolder.FullName, contentItem.UniqueId + ".md");
                        outputFilePath = Path.Combine(exportFilePath, contentItem.Author.Username.ToLowerInvariant(), contentItem.UniqueId + ".md");

                        FileInfo outputFile = new FileInfo(outputFilePath);

                        if (!outputFile.Directory.Exists)
                        {
                            outputFile.Directory.Create();
                        }

                        if (this.ExecutePandoc(inputTempHtmlFilePath, outputTempMarkdownFilePath))
                        {
                            sb.Append(await File.ReadAllTextAsync(outputTempMarkdownFilePath).ConfigureAwait(false));

                            string content = sb.ToString();

                            Console.WriteLine(outputFilePath);

                            try
                            {
                                content = this.cleanerManager.PostConvert(content);
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine(exception.Message);
                            }

                            await using (var writer = File.CreateText(outputFilePath))
                            {
                                await writer.WriteAsync(content).ConfigureAwait(false);
                            }
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

        private string CreateYamlHeader(ContentItem contentItem)
        {
            if (string.IsNullOrEmpty(contentItem.Slug))
            {
                contentItem.Slug = new string(Regex.Replace(contentItem.Content.Title.ToLowerInvariant().Replace(" ", "-"), @"\-+", "-").Where(ch => !Path.GetInvalidFileNameChars().Contains(ch)).ToArray());
            }

            var frontmatter = new
            {
                Title = contentItem.Content.Title.Replace("“", "\"").Replace("”", "\"").Replace("’", "'").Replace("‘", "'"),
                Date = contentItem.PublishedOn.ToString("O"),
                Author = contentItem.Author.Username,
                Category = contentItem.Categories,
                Tags = contentItem.Tags,
                Slug = contentItem.Slug,
                Status = contentItem.Status,
                HeaderImageUrl = this.GetHeaderImage(contentItem.Content.Attachments.Select(x => x.Path).Distinct().ToList()),
                Excerpt = contentItem.Content.Excerpt.Replace("\n", string.Empty).Replace("“", "\"").Replace("”", "\"").Replace("’", "'").Replace("‘", "'").Trim(),
                Attachments = contentItem.Content.Attachments.Select(x => x.Path).Distinct(),
            };

            return this.serializer.Serialize(frontmatter);
        }

        private List<ContentItem> LoadFeed(BlogSite blogSite)
        {
            Console.WriteLine($"Processing...");

            var feed = new List<ContentItem>();
            var settings = this.settingsManager.LoadSettings(nameof(StackerSettings));
            var posts = blogSite.GetAllPostsInAllPublicationStates().ToList();

            Console.WriteLine($"Total Posts: {posts.Count}");

            // var attachments = posts.Where(x => x.Attachments.Any());
            foreach (var post in posts)
            {
                var user = settings.Users.Find(u => string.Equals(u.Email, post.Author.Email, StringComparison.InvariantCultureIgnoreCase));

                var ci = new ContentItem
                {
                    Author = new AuthorDetails
                    {
                        DisplayName = post.Author.DisplayName,
                        Email = post.Author.Email,
                        TwitterHandle = user.Twitter,
                        Username = post.Author.Username,
                    },
                    Categories = post.Categories.Select(c => c.Name).Where(x => !this.IsCategoryExcluded(x)),
                    Content = new ContentDetails
                    {
                        Attachments = post.Attachments.Select(x => new ContentAttachment { Path = x.Path, Url = x.Url }).ToList(),
                        Body = post.Body,
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
                };

                // Search the body for any missing images.
                var matches = Regex.Matches(post.Body, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        if (!ci.Content.Attachments.Any(x => string.Equals(x.Url, match.Groups[1].Value, StringComparison.InvariantCultureIgnoreCase)) && this.IsRelevantHost(match.Groups[1].Value))
                        {
                            ci.Content.Attachments.Add(new ContentAttachment { Path = match.Groups[1].Value, Url = match.Groups[1].Value });
                        }
                    }
                }

                ci = this.cleanerManager.PreDownload(ci);

                feed.Add(ci);
            }

            return feed;
        }

        private async Task<BlogSite> LoadWordPressExportAsync(string wpexportFilePath)
        {
            BlogSite blogSite;

            Console.WriteLine($"Reading {wpexportFilePath}");

            using (var reader = File.OpenText(wpexportFilePath))
            {
                var document = await XDocument.LoadAsync(reader, LoadOptions.None, CancellationToken.None).ConfigureAwait(false);
                blogSite = new BlogSite(document);
            }

            return blogSite;
        }

        private bool ExecutePandoc(string inputTempHtmlFilePath, string outputTempMarkdownFilePath)
        {
            bool success = false;

            string arguments = $"-f html+raw_html --to=markdown_github-raw_html --wrap=preserve -o \"{outputTempMarkdownFilePath}\" \"{inputTempHtmlFilePath}\" ";

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

        private bool IsCategoryExcluded(string category)
        {
            string[] excluded = new string[] { "Uncategorized", "Mobile Services", "Networking", string.Empty };

            return excluded.Contains(category);
        }

        private string GetHeaderImage(List<string> attachments)
        {
            if (attachments.Count == 1)
            {
                return attachments[0];
            }

            var header = attachments.Find(x => x.Contains("header-", StringComparison.InvariantCultureIgnoreCase) || x.Contains("1024px", StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrEmpty(header))
            {
                return header.Trim();
            }

            return string.Empty;
        }

        private bool IsRelevantHost(string url)
        {
            var hosts = new string[] { "blogs.endjin.com", "endjinblog.azurewebsites.net" };

            return hosts.Any(x => url.Contains(x, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}