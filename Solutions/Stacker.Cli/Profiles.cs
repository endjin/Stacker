// <copyright file="Profiles.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli
{
    using Newtonsoft.Json;

    public class Profiles
    {
        [JsonProperty("profile_id")]
        public string ProfileId { get; set; }
    }
}