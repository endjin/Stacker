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

    public bool Promote { get; internal set; }

    public DateTimeOffset PromoteUntil { get; set; }

    public DateTimeOffset PublishedOn { get; set; }

    public string Slug { get; internal set; }

    public string CleanSlug
    {
        get
        {
            return Regex.Replace(this.Slug, @"\-+", "-");
        }
    }

    public string Status { get; internal set; }

    public List<string> Tags { get; set; }

    public string UniqueId
    {
        get
        {
            if (string.IsNullOrEmpty(this.Slug))
            {
                return this.Id;
            }
            else
            {
                return this.CleanSlug;
            }
        }
    }
}