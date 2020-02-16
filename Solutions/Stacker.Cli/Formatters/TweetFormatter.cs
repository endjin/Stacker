// <copyright file="TweetFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Formatters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Stacker.Cli.Contracts.Formatters;
    using Stacker.Cli.Domain.Universal;

    public class TweetFormatter : IContentFormatter
    {
        private const int MaxContentLength = 280;

        public IEnumerable<string> Format(IEnumerable<ContentItem> feedItems)
        {
            var tweets = new List<string>();
            var sb = new StringBuilder();

            foreach (var item in feedItems)
            {
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
                    var tweetLength = sb.Length + item.Content.Link.Length + 1; // 1 = extra space before link
                    int tagsToInclude = 0;

                    foreach (var tag in item.Tags)
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

                    foreach (var tag in item.Tags.Take(tagsToInclude))
                    {
                        sb.Append(" #");
                        sb.Append(tag);
                    }
                }

                sb.Append(" ");
                sb.Append(item.Content.Link);

                tweets.Add(sb.ToString());

                sb.Clear();
            }

            return tweets;
        }
    }
}