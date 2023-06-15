// <copyright file="BufferError.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stacker.Cli;

public class BufferError
{
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("code")]
    public int Code { get; set; }

    [JsonProperty("errored_profiles")]
    public IEnumerable<Profiles> ErroredProfiles { get; set; }
}