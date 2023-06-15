// <copyright file="Category.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Diagnostics;

namespace Stacker.Cli.Domain.WordPress;

[DebuggerDisplay("{Name} Id = {Id}")]
public class Category
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Slug { get; set; }
}