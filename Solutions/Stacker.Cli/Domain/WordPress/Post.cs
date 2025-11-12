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
        this.Attachments = [];
        this.Categories = [];
        this.Tags = [];
    }

    public IEnumerable<Attachment> Attachments { get; internal set; }

    public required Author Author { get; set; }

    public required string Body { get; set; }

    public IEnumerable<Category> Categories { get; set; }

    public required string Excerpt { get; set; }

    public Attachment? FeaturedImage { get; set; }

    public required string Id { get; set; }

    public required string Link { get; set; }

    public Dictionary<string, string> MetaData { get; set; } = [];

    public bool? Promote { get; set; }

    public DateTimeOffset? PromoteUntil { get; set; } = DateTimeOffset.MaxValue;

    public DateTimeOffset PublishedAtUtc { get; set; }

    public required string Slug { get; set; }

    public required string Status { get; set; }

    public IEnumerable<Tag> Tags { get; set; }

    public required string Title { get; set; }
}