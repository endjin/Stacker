// <copyright file="BufferClient.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Stacker.Cli.Configuration;
using Stacker.Cli.Contracts.Buffer;
using Stacker.Cli.Contracts.Configuration;

namespace Stacker.Cli;

public class BufferClient : IBufferClient
{
    private const string BaseUri = "https://api.bufferapp.com/1/";
    private const string UpdateOperation = "updates/create.json";

    private readonly IStackerSettingsManager settingsManager;
    private readonly IHttpClientFactory httpClientFactory;

    public BufferClient(IStackerSettingsManager settingsManager, IHttpClientFactory httpClientFactory)
    {
        this.settingsManager = settingsManager;
        this.httpClientFactory = httpClientFactory;
    }

    public IEnumerable<KeyValuePair<string, string>> ConvertToPayload(string content, string[] profileIds)
    {
        var postData = new List<KeyValuePair<string, string>>
        {
            new("text", content),
            new("shorten", "false"),
        };

        postData.AddRange(profileIds.Select(profileId => new KeyValuePair<string, string>("profile_ids[]", profileId)));

        return postData;
    }

    public async Task UploadAsync(IEnumerable<string> content, string profileId)
    {
        using (HttpClient client = this.httpClientFactory.CreateClient())
        {
            client.BaseAddress = new(BaseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new("application/x-www-form-urlencoded"));

            StackerSettings settings = this.settingsManager.LoadSettings(nameof(StackerSettings));
            string updateOperationUrl = $"{UpdateOperation}?access_token={settings.BufferAccessToken}";

            foreach (string item in content)
            {
                HttpContent payload = new FormUrlEncodedContent(this.ConvertToPayload(item, new string[] { profileId }));

                Console.WriteLine($"Buffering: {item}");

                HttpResponseMessage response = await client.PostAsync(updateOperationUrl, payload).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    BufferError error = JsonConvert.DeserializeObject<BufferError>(errorContent);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Buffering Failed: {error.Message}");
                    Console.WriteLine();
                    Console.ResetColor();
                }
            }
        }
    }
}