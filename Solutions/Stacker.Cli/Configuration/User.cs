// <copyright file="User.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Diagnostics;

namespace Stacker.Cli.Configuration;

[DebuggerDisplay("{Email} IsActive = {IsActive}")]
public class User
{
    public required string Email { get; set; }

    public bool IsActive { get; set; }

    public required string Twitter { get; set; }
}