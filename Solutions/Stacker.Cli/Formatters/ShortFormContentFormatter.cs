// <copyright file="ShortFormContentFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Stacker.Cli.Configuration;
using Stacker.Cli.Contracts.Formatters;
using Stacker.Cli.Domain.Universal;

namespace Stacker.Cli.Formatters;

public class ShortFormContentFormatter : IContentFormatter
{
    private readonly int maxContentLength;
    private readonly string campaignSource;

    public ShortFormContentFormatter(int maxContentLength, string campaignSource)
    {
        this.campaignSource = campaignSource;
        this.maxContentLength = maxContentLength;
    }

    public IEnumerable<string> Format(string campaignMedium, string campaignName, IEnumerable<ContentItem> feedItems, StackerSettings settings)
    {
        List<string> socialMediaPosts = [];
        StringBuilder content = new();
        StringBuilder campaignTracking = new();

        foreach (ContentItem item in feedItems)
        {
            campaignTracking.Append(' ');
            campaignTracking.Append(item.Content.Link);
            campaignTracking.Append("?utm_source=");
            campaignTracking.Append(this.campaignSource.ToLowerInvariant());
            campaignTracking.Append("&utm_medium=");
            campaignTracking.Append(campaignMedium.ToLowerInvariant());
            campaignTracking.Append("&utm_campaign=");
            campaignTracking.Append(campaignName.ToLowerInvariant());
            campaignTracking.AppendLine();

            List<HashTag> titleMatches = [];

            string title = item.Content.Title;

            if (item.HashTags is not null)
            {
                foreach (HashTag hashTag in item.HashTags?.Where(x => !x.Default))
                {
                    if (title.Contains(hashTag.Text, StringComparison.InvariantCultureIgnoreCase))
                    {
                        titleMatches.Add(hashTag);
                        string pattern = $@"\b{Regex.Escape(hashTag.Text)}\b";
                        title = Regex.Replace(title, pattern, hashTag.Tag, RegexOptions.IgnoreCase);
                    }
                }

                foreach (HashTag hashTag in titleMatches)
                {
                    item.HashTags.Remove(hashTag);
                }
            }

            content.Append(title);

            User match = settings.Users.Find(x => string.Equals(item.Author.Email, x.Email, StringComparison.InvariantCultureIgnoreCase));

            if (match?.IsActive == true)
            {
                content.Append(" by ");

                if (string.IsNullOrEmpty(item.Author.TwitterHandle))
                {
                    content.Append(item.Author.DisplayName);
                }
                else
                {
                    content.Append('@');
                    content.Append(item.Author.TwitterHandle);
                }
            }

            // If we don't find a match from our users, just use the display name
            if (match is null)
            {
                content.Append(" by ");
                content.Append(item.Author.DisplayName);
            }

            if (item?.Tags is { Count: > 0 })
            {
                int tweetLength = content.Length + campaignTracking.Length + 1; // 1 = extra space before link
                int tagsToInclude = 0;

                foreach (string tag in item.Tags)
                {
                    tweetLength += tag.Length + 2; // 2 - Offset = Space + #
                    if (tweetLength <= this.maxContentLength)
                    {
                        tagsToInclude++;
                    }
                    else
                    {
                        break;
                    }
                }

                foreach (string tag in item.Tags.Take(tagsToInclude))
                {
                    content.Append(" #");
                    content.Append(tag);
                }
            }

            if (item?.HashTags is { Count: > 0 })
            {
                int tweetLength = content.Length + campaignTracking.Length + 1; // 1 = extra space before link
                int tagsToInclude = 0;

                foreach (HashTag tag in item.HashTags)
                {
                    tweetLength += tag.Tag.Length + 1; // 1 - Offset = Space
                    if (tweetLength <= this.maxContentLength)
                    {
                        tagsToInclude++;
                    }
                    else
                    {
                        break;
                    }
                }

                foreach (HashTag tag in item.HashTags.Take(tagsToInclude))
                {
                    content.Append(" ");
                    content.Append(tag.Tag);
                }
            }

            content.Append(campaignTracking);

            socialMediaPosts.Add(content.ToString());

            content.Clear();
            campaignTracking.Clear();
        }

        return socialMediaPosts;
    }
}