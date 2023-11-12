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
    public AuthorDetails Author { get; set; }

    public IEnumerable<string> Categories { get; internal set; }

    public ContentDetails Content { get; set; }

    public string Id { get; internal set; }

    public bool? Promote { get; set; }

    public DateTimeOffset? PromoteUntil { get; set; }

    public DateTimeOffset PublishedOn { get; set; }

    public string Slug { get; internal set; }

    public string CleanSlug
    {
        get
        {
            return Regex.Replace(this.Slug, @"\-+", "-");
        }
    }

    public string Status { get; set; }

    public List<string> Tags { get; set; }

    public string UniqueId
    {
        get
        {
            return string.IsNullOrEmpty(this.Slug) ? this.Id : this.CleanSlug;
        }
    }
}