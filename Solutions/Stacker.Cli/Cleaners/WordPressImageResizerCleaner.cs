﻿// <copyright file="WordPressImageResizerCleaner.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Cleaners
{
    using System.Text.RegularExpressions;
    using Stacker.Cli.Domain.Universal;

    public class WordPressImageResizerCleaner : IPreDownloadCleaner
    {
        public ContentItem Clean(ContentItem contentItem)
        {
            string pattern = @"(-\d+?x\d+?|(_thumb(\d+?)?))(?=.png|.jpg)";

            contentItem.Content.Body = Regex.Replace(contentItem.Content.Body, pattern, string.Empty);

            foreach (var attachment in contentItem.Content.Attachments)
            {
                attachment.Path = Regex.Replace(attachment.Path, pattern, string.Empty);
                attachment.Url = Regex.Replace(attachment.Url, pattern,  string.Empty);
            }

            return contentItem;
        }
    }
}
