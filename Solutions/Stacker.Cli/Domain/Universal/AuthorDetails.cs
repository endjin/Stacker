// <copyright file="AuthorDetails.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.Universal;

public class AuthorDetails
{
    public string DisplayName { get; set; }

    public string Email { get; set; }

    public string TwitterHandle { get; set; }

    public string Username { get; internal set; }
}