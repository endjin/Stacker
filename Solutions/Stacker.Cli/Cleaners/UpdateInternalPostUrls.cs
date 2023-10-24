// <copyright file="UpdateInternalPostUrls.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Text.RegularExpressions;

namespace Stacker.Cli.Cleaners;

public class UpdateInternalPostUrls : IPostConvertCleaner
{
    public string Clean(string content)
    {
        Regex regexp = new(@"\((\/\d{4}\/\d{2}\/.*?)(\/)", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

        content = regexp.Replace(content, (match) =>
        {
            Group group = match.Groups[1];
            return $"(/blog{group.Value}.html";
        });

        return content;
    }
}