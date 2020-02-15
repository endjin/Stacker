﻿// <copyright file="FacebookFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.LinkedIn
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Stacker.Cli.Domain.Universal;

    public class FacebookFormatter
    {
        private const int MaxContentLength = 1290;

        public IEnumerable<string> Format(IEnumerable<FeedItem> feedItems)
        {
            var postings = new List<string>();
            var sb = new StringBuilder();

            foreach (var item in feedItems)
            {
                sb.Append(item.Content.Excerpt);

                if (item.Tags != null && item.Tags.Any())
                {
                    var contentLength = sb.Length + item.Content.Link.Length + 1; // 1 = extra space before link
                    int tagsToInclude = 0;

                    foreach (var tag in item.Tags)
                    {
                        // 2 Offset = Space + #
                        if (contentLength + tag.Length + 2 <= MaxContentLength)
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
                        sb.Append(tag);
                        sb.AppendLine();
                    }
                }

                sb.AppendLine();
                sb.Append(item.Content.Link);
                sb.AppendLine();

                postings.Add(sb.ToString());

                sb.Clear();
            }

            return postings;
        }
    }
}