// <copyright file="Tag.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Diagnostics;

namespace Stacker.Cli.Domain.WordPress;

[DebuggerDisplay("{Slug} Id = {Id}")]
public class Tag
{
    public required string Id { get; set; }

    public required string Name { get; set; }

    public required string Slug { get; set; }
}