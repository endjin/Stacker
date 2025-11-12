// <copyright file="ContentItem.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Stacker.Cli.Domain.Universal;

[DebuggerDisplay("{Content.Title} by {Author.DisplayName}")]
public class ContentItem
{
    public required AuthorDetails Author { get; set; }

    public IEnumerable<string> Categories { get; internal set; } = [];

    public required ContentDetails Content { get; set; }

    public required string Id { get; set; }

    public bool? Promote { get; set; }

    public DateTimeOffset? PromoteUntil { get; set; }

    public DateTimeOffset PublishedOn { get; set; }

    public required string Slug { get; set; }

    public string CleanSlug
    {
        get
        {
            return Regex.Replace(this.Slug, @"\-+", "-");
        }
    }

    public required string Status { get; set; }

    public List<string> Tags { get; set; } = [];

    public List<HashTag> HashTags { get; set; } = [];

    public string UniqueId
    {
        get
        {
            return string.IsNullOrEmpty(this.Slug) ? this.Id : this.CleanSlug;
        }
    }
}