// <copyright file="Category.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Diagnostics;

namespace Stacker.Cli.Domain.WordPress;

[DebuggerDisplay("{Name} Id = {Id}")]
public class Category
{
    public required string Id { get; set; }

    public required string Name { get; set; }

    public required string Slug { get; set; }
}