// <copyright file="BufferClient.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Spectre.Console;

using Stacker.Cli.Configuration;
using Stacker.Cli.Contracts.Buffer;

namespace Stacker.Cli;

public class BufferClient : IBufferClient
{
    private const string BaseUri = "https://api.bufferapp.com/1/";
    private const string UpdateOperation = "updates/create.json";
    private const string ShuffleOperationFormat = "profiles/{0}/updates/shuffle.json";

    private readonly IHttpClientFactory httpClientFactory;
    private readonly StackerSettings settings;

    public BufferClient(IHttpClientFactory httpClientFactory, StackerSettings settings)
    {
        this.httpClientFactory = httpClientFactory;
        this.settings = settings;
    }

    public async Task UploadAsync(IEnumerable<string> content, string profileId, bool whatIf)
    {
        using HttpClient client = this.httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(BaseUri);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

        string updateOperationUrl = $"{UpdateOperation}?access_token={this.settings.BufferAccessToken}";

        // Convert to list to get count and maintain index
        var contentList = content.ToList();
        int totalCount = contentList.Count;
        int currentIndex = 0;

        foreach (string item in contentList)
        {
            currentIndex++;
            AnsiConsole.MarkupLineInterpolated($"[darkturquoise][[{currentIndex}/{totalCount}]][/] [chartreuse3_1]Buffering:[/] {item}");

            if (whatIf)
            {
                continue;
            }

            HttpContent payload = new FormUrlEncodedContent(this.ConvertToPayload(item, [profileId]));

            HttpResponseMessage response = await client.PostAsync(updateOperationUrl, payload).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                BufferError error = JsonSerializer.Deserialize<BufferError>(errorContent);

                AnsiConsole.MarkupLineInterpolated($"[red]Buffering Failed:[/] {error.Message}");
                AnsiConsole.WriteLine();
            }
        }
    }

    public async Task<BufferShuffleResponse> ShuffleAsync(string profileId, int? count = null, bool? utc = null)
    {
        using HttpClient client = this.httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(BaseUri);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

        string shuffleOperationUrl = $"{string.Format(ShuffleOperationFormat, profileId)}?access_token={this.settings.BufferAccessToken}";

        AnsiConsole.MarkupLineInterpolated($"[chartreuse3_1]Shuffling Buffer queue for profile:[/] {profileId}");

        List<KeyValuePair<string, string>> postData = [];

        if (count.HasValue)
        {
            postData.Add(new("count", count.Value.ToString()));
        }

        if (utc.HasValue)
        {
            postData.Add(new("utc", utc.Value.ToString().ToLower()));
        }

        HttpContent payload = new FormUrlEncodedContent(postData);
        HttpResponseMessage response = await client.PostAsync(shuffleOperationUrl, payload).ConfigureAwait(false);

        string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            BufferError error = JsonSerializer.Deserialize<BufferError>(content);
            AnsiConsole.MarkupLineInterpolated($"[red]Shuffling Failed:[/] {error?.Message}");
            AnsiConsole.WriteLine();
            return new BufferShuffleResponse { Success = false };
        }

        BufferShuffleResponse result = JsonSerializer.Deserialize<BufferShuffleResponse>(content);
        AnsiConsole.MarkupLine("[chartreuse3_1]Shuffling completed successfully[/]");

        return result ?? new BufferShuffleResponse { Success = false };
    }

    private IEnumerable<KeyValuePair<string, string>> ConvertToPayload(string content, string[] profileIds)
    {
        List<KeyValuePair<string, string>> postData =
        [
            new("text", content),
            new("shorten", "false")
        ];

        postData.AddRange(profileIds.Select(profileId => new KeyValuePair<string, string>("profile_ids[]", profileId)));

        return postData;
    }
}