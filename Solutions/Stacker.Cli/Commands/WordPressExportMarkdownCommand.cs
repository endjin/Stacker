// <copyright file="WordPressExportMarkdownCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.IO;

using Stacker.Cli.Cleaners;
using Stacker.Cli.Configuration;
using Stacker.Cli.Contracts.Configuration;
using Stacker.Cli.Domain.Universal;
using Stacker.Cli.Domain.WordPress;
using Stacker.Cli.Serialization;
using Stacker.Cli.Tasks;

using YamlDotNet.Serialization;

using Environment = System.Environment;
using Path = System.IO.Path;

namespace Stacker.Cli.Commands;

public class WordPressExportMarkdownCommand : AsyncCommand<WordPressExportMarkdownCommand.Settings>
{
    private readonly IDownloadTasks downloadTasks;
    private readonly IStackerSettingsManager settingsManager;
    private readonly ContentItemCleaner cleanerManager;
    private readonly IYamlSerializerFactory serializerFactory;
    private ISerializer serializer;
    private StackerSettings stackerSettings;

    public WordPressExportMarkdownCommand(IDownloadTasks downloadTasks, IStackerSettingsManager settingsManager, ContentItemCleaner cleanerManager, IYamlSerializerFactory serializerFactory)
    {
        this.downloadTasks = downloadTasks;
        this.settingsManager = settingsManager;
        this.cleanerManager = cleanerManager;
        this.serializerFactory = serializerFactory;
    }

    /// <inheritdoc/>
    public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        this.stackerSettings = this.settingsManager.LoadSettings(nameof(StackerSettings));

        if (!File.Exists(settings.WordPressExportFilePath.FullPath))
        {
            AnsiConsole.WriteLine($"File not found {settings.WordPressExportFilePath.FullPath}");

            return 1;
        }

        this.serializer = this.serializerFactory.GetSerializer();

        BlogSite blogSite = await this.LoadWordPressExportAsync(settings.WordPressExportFilePath.FullPath).ConfigureAwait(false);

        List<ContentItem> feed = this.LoadFeed(blogSite);

        StringBuilder sb = new();
        FileInfo fi = new(settings.OutputDirectoryPath.FullPath);
        DirectoryInfo tempHtmlFolder = new(Path.Join(Path.GetTempPath(), "stacker", "html"));
        DirectoryInfo tempMarkdownFolder = new(Path.Join(Path.GetTempPath(), "stacker", "md"));

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
        foreach (ContentItem ci in feed)
        {
            ContentItem contentItem = this.cleanerManager.PostDownload(ci);

            sb.AppendLine("---");
            sb.Append(this.CreateYamlHeader(contentItem));
            sb.Append("---");
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);

            await using (StreamWriter writer = File.CreateText(Path.Combine(tempHtmlFolder.FullName, contentItem.UniqueId + ".html")))
            {
                await writer.WriteAsync(contentItem.Content.Body).ConfigureAwait(false);
            }

            inputTempHtmlFilePath = Path.Combine(tempHtmlFolder.FullName, contentItem.UniqueId + ".html");
            outputTempMarkdownFilePath = Path.Combine(tempMarkdownFolder.FullName, contentItem.UniqueId + ".md");
            outputFilePath = Path.Combine(settings.OutputDirectoryPath.FullPath, contentItem.Author.Username.ToLowerInvariant(), contentItem.UniqueId + ".md");

            FileInfo outputFile = new(outputFilePath);

            if (!outputFile.Directory.Exists)
            {
                outputFile.Directory.Create();
            }

            if (this.ExecutePandoc(inputTempHtmlFilePath, outputTempMarkdownFilePath))
            {
                sb.Append(await File.ReadAllTextAsync(outputTempMarkdownFilePath).ConfigureAwait(false));

                string content = sb.ToString();

                AnsiConsole.WriteLine(outputFilePath);

                try
                {
                    content = this.cleanerManager.PostConvert(content);
                }
                catch (Exception exception)
                {
                    AnsiConsole.WriteLine(exception.Message);
                }

                await using StreamWriter writer = File.CreateText(outputFilePath);
                await writer.WriteAsync(content).ConfigureAwait(false);
            }

            // Remote the temporary html file.
            File.Delete(inputTempHtmlFilePath);

            sb.Clear();
        }

        return 0;
    }

    private string CreateYamlHeader(ContentItem contentItem)
    {
        if (string.IsNullOrEmpty(contentItem.Slug))
        {
            contentItem.Slug = new string(Regex.Replace(contentItem.Content.Title.ToLowerInvariant().Replace(" ", "-"), @"\-+", "-").Where(ch => !Path.GetInvalidFileNameChars().Contains(ch)).ToArray());
        }

        var frontMatter = new
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

        return this.serializer.Serialize(frontMatter);
    }

    private List<ContentItem> LoadFeed(BlogSite blogSite)
    {
        AnsiConsole.WriteLine("Processing...");

        List<ContentItem> feed = new();
        StackerSettings settings = this.settingsManager.LoadSettings(nameof(StackerSettings));
        List<Post> posts = blogSite.GetAllPostsInAllPublicationStates().ToList();

        AnsiConsole.WriteLine($"Total Posts: {posts.Count}");

        // var attachments = posts.Where(x => x.Attachments.Any());
        foreach (Post post in posts)
        {
            User user = settings.Users.Find(u => string.Equals(u.Email, post.Author.Email, StringComparison.InvariantCultureIgnoreCase));

            if (user is null)
            {
                throw new NotImplementedException($"User {post.Author.Email} has not been configured. Update the settings file.");
            }

            ContentItem ci = new()
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
                Tags = post.Tags.Where(t => t != null).Select(t => t.Name).ToList(),
            };

            // Search the body for any missing images.
            MatchCollection matches = Regex.Matches(post.Body, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

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

    private async Task<BlogSite> LoadWordPressExportAsync(string exportFilePath)
    {
        AnsiConsole.WriteLine($"Reading {exportFilePath}");

        using StreamReader reader = File.OpenText(exportFilePath);
        XDocument document = await XDocument.LoadAsync(reader, LoadOptions.None, CancellationToken.None).ConfigureAwait(false);
        BlogSite blogSite = new(document);

        return blogSite;
    }

    private bool ExecutePandoc(string inputTempHtmlFilePath, string outputTempMarkdownFilePath)
    {
        bool success = false;

        string arguments = $"-f html+raw_html --to=markdown_github-raw_html --wrap=preserve -o \"{outputTempMarkdownFilePath}\" \"{inputTempHtmlFilePath}\" ";

        ProcessStartInfo psi = new()
        {
            FileName = "pandoc",
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardInput = true,
        };

        Process process = new() { StartInfo = psi };
        process.Start();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            AnsiConsole.WriteLine("Failed to convert " + outputTempMarkdownFilePath);
            AnsiConsole.WriteLine(process.StandardError.ReadToEnd());
        }
        else
        {
            success = true;
        }

        return success;
    }

    private bool IsCategoryExcluded(string category)
    {
        return this.stackerSettings.WordPressToMarkdown.TagsToRemove.Contains(category);
    }

    private string GetHeaderImage(List<string> attachments)
    {
        if (attachments.Count == 1)
        {
            return attachments[0];
        }

        string header = attachments.Find(x => x.Contains("header-", StringComparison.InvariantCultureIgnoreCase) || x.Contains("1024px", StringComparison.InvariantCultureIgnoreCase));

        return !string.IsNullOrEmpty(header) ? header.Trim() : string.Empty;
    }

    private bool IsRelevantHost(string url)
    {
        return this.stackerSettings.WordPressToMarkdown.Hosts.Any(x => url.Contains(x, StringComparison.InvariantCultureIgnoreCase));
    }

    /// <summary>
    /// The settings for the command.
    /// </summary>
    public class Settings : CommandSettings
    {
        [CommandOption("-w|--wp-export-file-path <WordPressExportFilePath>")]
        [Description("WordPress Export file path.")]
        public FilePath WordPressExportFilePath { get; init; }

        [CommandOption("-o|--output-directory-path <OutputDirectoryPath>")]
        [Description("Directory path for the exported files.")]
        public DirectoryPath OutputDirectoryPath { get; init; }
    }
}