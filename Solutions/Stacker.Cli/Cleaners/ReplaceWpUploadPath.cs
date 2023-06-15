// <copyright file="ReplaceWpUploadPath.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Text.RegularExpressions;

namespace Stacker.Cli.Cleaners;

public class ReplaceWpUploadPath : IPostConvertCleaner
{
    public string Clean(string content)
    {
        Regex regexp = new Regex(@"\((\/wp-content\/uploads\/)", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

        return regexp.Replace(content, "(/assets/images/blog/");
    }
}