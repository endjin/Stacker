// <copyright file="ContentDetails.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Stacker.Cli.Domain.Universal;

public class ContentDetails
{
    public List<ContentAttachment> Attachments { get; internal set; } = [];

    public required string Body { get; set; }

    public required string Excerpt { get; set; }

    public required string Link { get; set; }

    public required string Title { get; set; }
}