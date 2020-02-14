// <copyright file="FeedItem.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.Universal
{
    using System;
    using System.Collections.Generic;

    public class FeedItem
    {
        public AuthorElement Author { get; set; }

        public Content Content { get; set; }

        public DateTimeOffset PromoteUntil { get; set; }

        public DateTimeOffset PublishedOn { get; set; }

        public IEnumerable<string> Tags { get; internal set; }
    }
}