// <copyright file="TweetFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.Twitter
{
    using System.Linq;
    using System.Text;

    public class TweetFormatter
    {
        private const int MaxTweetLength = 280;

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
                    if (tweetLength + tag.Length + 2 <= MaxTweetLength)
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
    }
}