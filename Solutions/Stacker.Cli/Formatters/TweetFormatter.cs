// <copyright file="TweetFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stacker.Cli.Configuration;
using Stacker.Cli.Contracts.Formatters;
using Stacker.Cli.Domain.Universal;

namespace Stacker.Cli.Formatters;

public class TweetFormatter : IContentFormatter
{
    private const int MaxContentLength = 280;
    private readonly string campaignSource = "twitter";

    public IEnumerable<string> Format(string campaignMedium, string campaignName, IEnumerable<ContentItem> feedItems, StackerSettings settings)
    {
        List<string> tweets = new();
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

            if (item?.Tags != null && item.Tags.Any())
            {
                int tweetLength = content.Length + (item.Content.Link.Length + 1) + campaignTracking.Length; // 1 = extra space before link
                int tagsToInclude = 0;

                item.Tags = item.Tags.Except(settings.ExcludedTags).OrderByDescending(word => settings.PriorityTags.IndexOf(word)).ToList();

                foreach (string tag in item.Tags.Distinct())
                {
                    // 2 Offset = Space + #
                    if (tweetLength + tag.Length + 2 <= MaxContentLength)
                    {
                        tagsToInclude++;
                    }
                    else
                    {
                        break;
                    }
                }

                foreach (string tag in item.Tags.Distinct().Take(tagsToInclude))
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