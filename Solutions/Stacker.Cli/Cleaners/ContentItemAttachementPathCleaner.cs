// <copyright file="ContentItemAttachementPathCleaner.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Cleaners
{
    using System;
    using System.Text.RegularExpressions;
    using Flurl;
    using Stacker.Cli.Domain.Universal;

    public class ContentItemAttachementPathCleaner : IPreDownloadCleaner
    {
        public ContentItem Clean(ContentItem contentItem)
        {
            string pattern = @"(https?:\/\/(?:(?:blogs?.endjin.com)|(?:endjinblog.azurewebsites.net))\/wp-content\/uploads)";
            string path = "/assets/images/blog";

            Regex regexp = new Regex(pattern, RegexOptions.Compiled, TimeSpan.FromSeconds(1));

            foreach (var attachment in contentItem.Content.Attachments)
            {
                attachment.Path = regexp.Replace(attachment.Path, path);

                if (!attachment.Path.StartsWith(path, StringComparison.InvariantCultureIgnoreCase))
                {
                    attachment.Path = Url.Combine(path, attachment.Path);
                }
            }

            return contentItem;
        }
    }
}
