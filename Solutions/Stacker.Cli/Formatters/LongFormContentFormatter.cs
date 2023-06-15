// <copyright file="LongFormContentFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stacker.Cli.Contracts.Formatters;
using Stacker.Cli.Converters;
using Stacker.Cli.Domain.Universal;

namespace Stacker.Cli.Formatters;

public abstract class LongFormContentFormatter : IContentFormatter
{
    private readonly int maxContentLength;
    private readonly string campaignSource;

    public LongFormContentFormatter(int maxContentLength, string campaignSource)
    {
        this.campaignSource = campaignSource;
        this.maxContentLength = maxContentLength;
    }

    public IEnumerable<string> Format(string campaignMedium, string campaignName, IEnumerable<ContentItem> feedItems)
    {
        var postings = new List<string>();
        var sb = new StringBuilder();
        var sbTracking = new StringBuilder();
        var hashTagConverter = new WordPressTagToHashTagConverter();

        foreach (var item in feedItems)
        {
            sbTracking.AppendLine();
            sbTracking.Append(item.Content.Link);
            sbTracking.Append("?utm_source=");
            sbTracking.Append(this.campaignSource.ToLowerInvariant());
            sbTracking.Append("&utm_medium=");
            sbTracking.Append(campaignMedium.ToLowerInvariant());
            sbTracking.Append("&utm_campaign=");
            sbTracking.Append(campaignName.ToLowerInvariant());
            sbTracking.AppendLine();

            sb.Append(item.Content.Excerpt);

            if (item.Tags?.Any() == true)
            {
                var contentLength = sb.Length + sbTracking.Length + 1; // 1 = extra space before link
                int tagsToInclude = 0;

                foreach (var tag in item.Tags)
                {
                    // 2 Offset = Space + #
                    if (contentLength + tag.Length + 2 <= this.maxContentLength)
                    {
                        tagsToInclude++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (item.Tags.Any())
                {
                    sb.AppendLine();
                    sb.AppendLine();
                }

                foreach (var tag in item.Tags.Take(tagsToInclude))
                {
                    sb.Append(" #");
                    sb.Append(hashTagConverter.Convert(tag));
                    sb.AppendLine();
                }
            }

            sb.Append(sbTracking.ToString());

            postings.Add(sb.ToString());

            sb.Clear();
            sbTracking.Clear();
        }

        return postings;
    }
}