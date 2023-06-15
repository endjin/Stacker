// <copyright file="WordPressToMarkdown.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Stacker.Cli.Configuration;

public class WordPressToMarkdown
{
    public List<string> Hosts { get; set; } = new List<string>();

    public List<string> TagsToRemove { get; set; } = new List<string>();
}