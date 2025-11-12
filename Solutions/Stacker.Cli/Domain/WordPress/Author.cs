// <copyright file="Author.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Diagnostics;

namespace Stacker.Cli.Domain.WordPress;

[DebuggerDisplay("{Email} Id = {Id}")]
public class Author
{
    public required string Id { get; set; }

    public required string Username { get; set; }

    public required string Email { get; set; }

    public required string DisplayName { get; set; }
}