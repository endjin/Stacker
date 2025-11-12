// <copyright file="Attachment.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Diagnostics;

namespace Stacker.Cli.Domain.WordPress;

[DebuggerDisplay("{Title} Id = {Id}")]
public class Attachment
{
    public required string Id { get; set; }

    public required string Path { get; set; }

    public required string PostId { get; set; }

    public required string Title { get; set; }

    public required string Url { get; set; }
}