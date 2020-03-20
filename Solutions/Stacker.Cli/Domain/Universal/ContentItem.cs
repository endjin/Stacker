// <copyright file="ContentItem.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.Universal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("{Content.Title} by {Author.DisplayName}")]
    public class ContentItem
    {
        public AuthorDetails Author { get; set; }

        public IEnumerable<string> Categories { get; internal set; }

        public ContentDetails Content { get; set; }

        public bool Promote { get; internal set; }

        public DateTimeOffset PromoteUntil { get; set; }

        public DateTimeOffset PublishedOn { get; set; }

        public string Slug { get; internal set; }

        public string Status { get; internal set; }

        public IEnumerable<string> Tags { get; set; }
    }
}