// <copyright file="AuthorDetails.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.Universal;

public class AuthorDetails
{
    public required string DisplayName { get; set; }

    public required string Email { get; set; }

    public required string TwitterHandle { get; set; }

    public required string Username { get; set; }
}