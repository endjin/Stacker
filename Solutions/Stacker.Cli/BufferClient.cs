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

    private readonly IHttpClientFactory httpClientFactory;
    private readonly StackerSettings settings;

    public BufferClient(IHttpClientFactory httpClientFactory, StackerSettings settings)
    {
        this.httpClientFactory = httpClientFactory;
        this.settings = settings;
    }

    public IEnumerable<KeyValuePair<string, string>> ConvertToPayload(string content, string[] profileIds)
    {
        List<KeyValuePair<string, string>> postData = new()
        {
            new KeyValuePair<string, string>("text", content),
            new KeyValuePair<string, string>("shorten", "false"),
        };

        postData.AddRange(profileIds.Select(profileId => new KeyValuePair<string, string>("profile_ids[]", profileId)));

        return postData;
    }

    public async Task UploadAsync(IEnumerable<string> content, string profileId, bool whatIf)
    {
        using HttpClient client = this.httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(BaseUri);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

        string updateOperationUrl = $"{UpdateOperation}?access_token={this.settings.BufferAccessToken}";

        foreach (string item in content)
        {
            AnsiConsole.MarkupLineInterpolated($"[chartreuse3_1]Buffering:[/] {item}");

            if (whatIf)
            {
                continue;
            }

            HttpContent payload = new FormUrlEncodedContent(this.ConvertToPayload(item, new string[] { profileId }));

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
}