// <copyright file="Tag.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Diagnostics;

namespace Stacker.Cli.Domain.WordPress;

[DebuggerDisplay("{Slug} Id = {Id}")]
public class Tag
{
    public string Id { get; set; }

    public string Name { get; internal set; }

    public string Slug { get; set; }
}