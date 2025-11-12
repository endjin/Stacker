// <copyright file="StackerSettings.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Stacker.Cli.Configuration;

public class StackerSettings
{
    /// <summary>
    /// Gets or sets the selected ADR template name.
    /// </summary>
    public List<User> Users { get; set; } = [];

    public required string BufferAccessToken { get; set; }

    public Dictionary<string, string> BufferProfiles { get; set; } = [];

    public List<string> ExcludedTags { get; set; } = [];

    public List<TagAliases> TagAliases { get; set; } = [];

    public List<string> PriorityTags { get; set; } = [];

    public required WordPressToMarkdown WordPressToMarkdown { get; set; }
}