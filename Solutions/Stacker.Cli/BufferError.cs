// <copyright file="BufferError.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Stacker.Cli;

public class BufferError
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("errored_profiles")]
    public IEnumerable<Profiles> ErroredProfiles { get; set; } = [];
}