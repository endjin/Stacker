// <copyright file="ReplaceWpUploadPath.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Cleaners
{
    using System;
    using System.Text.RegularExpressions;

    public class ReplaceWpUploadPath : IPostConvertCleaner
    {
        public string Clean(string content)
        {
            Regex regexp = new Regex(@"\((\/wp-content\/uploads\/)", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

            return regexp.Replace(content, "(/assets/images/blog/");
        }
    }
}