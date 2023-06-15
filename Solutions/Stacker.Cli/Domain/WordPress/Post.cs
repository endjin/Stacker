// <copyright file="Post.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace Stacker.Cli.Domain.WordPress;

public class Post
{
    public Post()
    {
        this.Attachments = Enumerable.Empty<Attachment>();
        this.Categories = Enumerable.Empty<Category>();
        this.Tags = Enumerable.Empty<Tag>();
    }

    public IEnumerable<Attachment> Attachments { get; internal set; }

    public Author Author { get; set; }

    public string Body { get; set; }

    public IEnumerable<Category> Categories { get; set; }

    public string Excerpt { get; set; }

    public Attachment FeaturedImage { get; set; }

    public string Id { get; internal set; }

    public string Link { get; set; }

    public Dictionary<string, string> MetaData { get; set; }

    public bool Promote { get; set; }

    public DateTimeOffset PromoteUntil { get; set; } = DateTimeOffset.MaxValue;

    public DateTimeOffset PublishedAtUtc { get; set; }

    public string Slug { get; set; }

    public string Status { get; internal set; }

    public IEnumerable<Tag> Tags { get; set; }

    public string Title { get; set; }
}