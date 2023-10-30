// <copyright file="Profiles.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Text.Json.Serialization;

namespace Stacker.Cli;

public class Profiles
{
    [JsonPropertyName("profile_id")]
    public string ProfileId { get; set; }
}