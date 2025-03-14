// <copyright file="WordPressToMarkdown.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Stacker.Cli.Configuration;

public class WordPressToMarkdown
{
    public List<string> Hosts { get; set; } = [];

    public List<string> TagsToRemove { get; set; } = [];
}