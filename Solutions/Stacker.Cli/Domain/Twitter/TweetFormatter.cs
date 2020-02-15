// <copyright file="TweetFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.Twitter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Stacker.Cli.Domain.Universal;

    public class TweetFormatter
    {
        private const int MaxContentLength = 280;

        public string Format(Tweet tweet)
        {
            var sb = new StringBuilder();

            sb.Append(tweet.Title);
            sb.Append(" by ");

            if (string.IsNullOrEmpty(tweet.AuthorHandle))
            {
                sb.Append(tweet.AuthorDisplaName);
            }
            else
            {
                sb.Append("@");
                sb.Append(tweet.AuthorHandle);
            }

            if (tweet.Tags != null && tweet.Tags.Any())
            {
                var tweetLength = sb.Length + tweet.Link.Length + 1; // 1 = extra space before link
                int tagsToInclude = 0;

                foreach (var tag in tweet.Tags)
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

                foreach (var tag in tweet.Tags.Take(tagsToInclude))
                {
                    sb.Append(" #");
                    sb.Append(tag);
                }
            }

            sb.Append(" ");
            sb.Append(tweet.Link);

            return sb.ToString();
        }

        public List<string> Format(IEnumerable<FeedItem> feedItems)
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