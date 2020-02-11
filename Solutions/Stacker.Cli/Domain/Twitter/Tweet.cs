// <copyright file="Tweet.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.Twitter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("{Title} by {AuthorDisplaName}")]
    public class Tweet
    {
        public string AuthorDisplaName { get; set; }

        public string AuthorHandle { get; set; }

        public DateTimeOffset PublishedOn { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public IEnumerable<string> Tags { get; internal set; }
    }
}