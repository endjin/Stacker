﻿// <copyright file="RemoveHostNamesFromBody.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Text.RegularExpressions;

using Stacker.Cli.Domain.Universal;

namespace Stacker.Cli.Cleaners;

public class RemoveHostNamesFromBody : IPostDownloadCleaner
{
    public ContentItem Clean(ContentItem contentItem)
    {
        string pattern = @"(https?:\/\/(?:(?:blogs?.endjin.com)|(?:endjinblog.azurewebsites.net)))";

        Regex regexp = new(pattern, RegexOptions.Compiled, TimeSpan.FromSeconds(1));

        contentItem.Content.Body = regexp.Replace(contentItem.Content.Body, string.Empty);

        return contentItem;
    }
}