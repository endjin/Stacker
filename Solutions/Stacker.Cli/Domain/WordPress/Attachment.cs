// <copyright file="Attachment.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Diagnostics;

namespace Stacker.Cli.Domain.WordPress;

[DebuggerDisplay("{Title} Id = {Id}")]
public class Attachment
{
    public string Id { get; set; }

    public string Path { get; internal set; }

    public string PostId { get; internal set; }

    public string Title { get; set; }

    public string Url { get; set; }
}