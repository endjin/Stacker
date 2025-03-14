// <copyright file="BufferUpdate.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Text.Json.Serialization;

namespace Stacker.Cli.Contracts.Buffer;

public class BufferUpdate
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("day")]
    public string Day { get; set; }

    [JsonPropertyName("due_at")]
    public long DueAt { get; set; }

    [JsonPropertyName("due_time")]
    public string DueTime { get; set; }

    [JsonPropertyName("profile_id")]
    public string ProfileId { get; set; }

    [JsonPropertyName("profile_service")]
    public string ProfileService { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("text_formatted")]
    public string TextFormatted { get; set; }

    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    [JsonPropertyName("via")]
    public string Via { get; set; }
}