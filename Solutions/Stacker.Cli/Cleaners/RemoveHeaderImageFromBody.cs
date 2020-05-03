// <copyright file="RemoveHeaderImageFromBody.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Cleaners
{
    using System;
    using System.Text.RegularExpressions;

    public class RemoveHeaderImageFromBody : IPostConvertCleaner
    {
        public string Clean(string content)
        {
            Regex regexp = new Regex(@"(\[?!\[.*\]\(.*\))", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

            return regexp.Replace(content, string.Empty, 1, 0);
        }
    }
}
