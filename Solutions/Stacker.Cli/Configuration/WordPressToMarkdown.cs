// <copyright file="WordPressToMarkdown.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Configuration
{
    using System.Collections.Generic;

    public class WordPressToMarkdown
    {
        public List<string> Hosts { get; set; } = new List<string>();

        public List<string> TagsToRemove { get; set; } = new List<string>();
    }
}