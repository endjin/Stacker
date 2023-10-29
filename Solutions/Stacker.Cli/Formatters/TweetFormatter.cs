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
        StringBuilder sb = new();
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

            sb.Append(item.Content.Title);
            sb.Append(" by ");

            if (string.IsNullOrEmpty(item.Author.TwitterHandle))
            {
                sb.Append(item.Author.DisplayName);
            }
            else
            {
                sb.Append('@');
                sb.Append(item.Author.TwitterHandle);
            }

            if (item?.Tags != null && item.Tags.Any())
            {
                int tweetLength = sb.Length + item.Content.Link.Length + 1; // 1 = extra space before link
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
                    sb.Append(" #");
                    sb.Append(tag);
                }
            }

            sb.Append(campaignTracking);

            tweets.Add(sb.ToString());

            sb.Clear();
            campaignTracking.Clear();
        }

        return tweets;
    }
}