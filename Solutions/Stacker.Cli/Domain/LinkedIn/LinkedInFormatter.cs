// <copyright file="LinkedInFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.LinkedIn
{
    using System.Linq;
    using System.Text;

    public class LinkedInFormatter
    {
        private const int MaxContentLength = 1290;

        public string Format(Posting posting)
        {
            var sb = new StringBuilder();

            sb.Append(posting.Body);

            if (posting.Tags != null && posting.Tags.Any())
            {
                var contentLength = sb.Length + posting.Link.Length + 1; // 1 = extra space before link
                int tagsToInclude = 0;

                foreach (var tag in posting.Tags)
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

                foreach (var tag in posting.Tags.Take(tagsToInclude))
                {
                    sb.Append(" #");
                    sb.Append(tag);
                }
            }

            sb.AppendLine();
            sb.Append(posting.Link);
            sb.AppendLine();

            return sb.ToString();
        }
    }
}