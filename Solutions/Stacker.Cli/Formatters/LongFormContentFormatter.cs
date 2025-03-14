// <copyright file="LongFormContentFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Text;

using Stacker.Cli.Configuration;
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

    public IEnumerable<string> Format(string campaignMedium, string campaignName, IEnumerable<ContentItem> feedItems, StackerSettings settings)
    {
        var postings = new List<string>();
        var content = new StringBuilder();
        var campaignTracking = new StringBuilder();
        var hashTagConverter = new TagToHashTagConverter();

        foreach (ContentItem item in feedItems)
        {
            campaignTracking.AppendLine();
            campaignTracking.Append(item.Content.Link);
            campaignTracking.Append("?utm_source=");
            campaignTracking.Append(this.campaignSource.ToLowerInvariant());
            campaignTracking.Append("&utm_medium=");
            campaignTracking.Append(campaignMedium.ToLowerInvariant());
            campaignTracking.Append("&utm_campaign=");
            campaignTracking.Append(campaignName.ToLowerInvariant());
            campaignTracking.AppendLine();

            content.Append(item.Content.Excerpt);

            if (item?.Tags != null && item.Tags.Any())
            {
                int contentLength = content.Length + campaignTracking.Length + 1; // 1 = extra space before link
                int tagsToInclude = 0;

                foreach (string tag in item.Tags)
                {
                    contentLength += tag.Length + 2; // 2 Offset = Space + #
                    if (contentLength <= this.maxContentLength)
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
                    content.AppendLine();
                    content.AppendLine();
                }

                foreach (string tag in item.Tags.Take(tagsToInclude))
                {
                    content.Append(" #");
                    content.Append(hashTagConverter.Convert(tag));
                    content.AppendLine();
                }
            }

            content.Append(campaignTracking.ToString());

            postings.Add(content.ToString());

            content.Clear();
            campaignTracking.Clear();
        }

        return postings;
    }
}