// <copyright file="EnsureEndjinHttpsInBody.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Cleaners
{
    using System;
    using System.Text.RegularExpressions;
    using Stacker.Cli.Domain.Universal;

    public class EnsureEndjinHttpsInBody : IPreDownloadCleaner
    {
        public ContentItem Clean(ContentItem contentItem)
        {
            string pattern = @"(http:\/\/endjin.com)";

            Regex regexp = new Regex(pattern, RegexOptions.Compiled, TimeSpan.FromSeconds(1));

            contentItem.Content.Body = regexp.Replace(contentItem.Content.Body, "https://endjin.com");

            return contentItem;
        }
    }
}
