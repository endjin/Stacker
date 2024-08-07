// <copyright file="ShortFormContentFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        List<string> tweets = [];
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

            content.Append(item.Content.Title);

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

            if (item?.Tags != null && item.Tags.Any())
            {
                int tweetLength = content.Length + campaignTracking.Length + 1; // 1 = extra space before link
                int tagsToInclude = 0;

                foreach (string tag in item.Tags)
                {
                    tweetLength += tag.Length + 2; // 2 Offset = Space + #
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

            content.Append(campaignTracking);

            tweets.Add(content.ToString());

            content.Clear();
            campaignTracking.Clear();
        }

        return tweets;
    }
}