// <copyright file="Author.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Diagnostics;

namespace Stacker.Cli.Domain.WordPress;

[DebuggerDisplay("{Email} Id = {Id}")]
public class Author
{
    public string Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string DisplayName { get; set; }
}