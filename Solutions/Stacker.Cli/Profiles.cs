// <copyright file="Profiles.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace Stacker.Cli;

public class Profiles
{
    [JsonProperty("profile_id")]
    public string ProfileId { get; set; }
}