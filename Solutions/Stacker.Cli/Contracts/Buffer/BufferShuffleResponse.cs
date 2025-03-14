// <copyright file="BufferShuffleResponse.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Stacker.Cli.Contracts.Buffer
{
    public class BufferShuffleResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("updates")]
        public List<BufferUpdate> Updates { get; set; }
    }
}