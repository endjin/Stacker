// <copyright file="TweetFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stacker.Cli.Contracts.Formatters;
using Stacker.Cli.Converters;
using Stacker.Cli.Domain.Universal;

namespace Stacker.Cli.Formatters;

public class TweetFormatter : IContentFormatter
{
    private const int MaxContentLength = 280;
    private string campaignSource = "twitter";

    public IEnumerable<string> Format(string campaignMedium, string campaignName, IEnumerable<ContentItem> feedItems)
    {
        var tweets = new List<string>();
        var sb = new StringBuilder();
        var sbTracking = new StringBuilder();
        var hashTagConverter = new WordPressTagToHashTagConverter();

        foreach (ContentItem item in feedItems)
        {
            sbTracking.Append(" ");
            sbTracking.Append(item.Content.Link);
            sbTracking.Append("?utm_source=");
            sbTracking.Append(this.campaignSource.ToLowerInvariant());
            sbTracking.Append("&utm_medium=");
            sbTracking.Append(campaignMedium.ToLowerInvariant());
            sbTracking.Append("&utm_campaign=");
            sbTracking.Append(campaignName.ToLowerInvariant());
            sbTracking.AppendLine();

            sb.Append(item.Content.Title);
            sb.Append(" by ");

            if (string.IsNullOrEmpty(item.Author.TwitterHandle))
            {
                sb.Append(item.Author.DisplayName);
            }
            else
            {
                sb.Append("@");
                sb.Append(item.Author.TwitterHandle);
            }

            if (item.Tags != null && item.Tags.Any())
            {
                int tweetLength = sb.Length + item.Content.Link.Length + 1; // 1 = extra space before link
                int tagsToInclude = 0;

                foreach (string tag in item.Tags)
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

                foreach (string tag in item.Tags.Take(tagsToInclude))
                {
                    sb.Append(" #");
                    sb.Append(hashTagConverter.Convert(tag));
                }
            }

            sb.Append(sbTracking.ToString());

            tweets.Add(sb.ToString());

            sb.Clear();
            sbTracking.Clear();
        }

        return tweets;
    }
}