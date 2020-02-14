// <copyright file="Post.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.WordPress
{
    using System;
    using System.Collections.Generic;

    public class Post
    {
        public Post()
        {
            this.Categories = System.Linq.Enumerable.Empty<Category>();
            this.Tags = System.Linq.Enumerable.Empty<Tag>();
        }

        public Author Author { get; set; }

        public string Body { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public string Excerpt { get; set; }

        public string Slug { get; set; }

        public Attachment FeaturedImage { get; set; }

        public Dictionary<string, string> MetaData { get; set; }

        public string Link { get; set; }

        public bool Promote { get; set; }

        public DateTimeOffset PromoteUntil { get; set; } = DateTimeOffset.MaxValue;

        public DateTimeOffset PublishedAtUtc { get; set; }

        public IEnumerable<Tag> Tags { get; set; }

        public string Title { get; set; }
    }
}