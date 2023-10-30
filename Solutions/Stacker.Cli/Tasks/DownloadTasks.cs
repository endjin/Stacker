// <copyright file="DownloadTasks.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using Corvus.Retry;
using Corvus.Retry.Policies;
using Corvus.Retry.Strategies;

using Spectre.Console;

using Stacker.Cli.Domain.Universal;

namespace Stacker.Cli.Tasks;

public class DownloadTasks : IDownloadTasks
{
    private readonly IHttpClientFactory httpClientFactory;

    public DownloadTasks(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task DownloadAsync(List<ContentItem> feed, string outputPath)
    {
        var downloadFeedBlock = new ActionBlock<DataflowContext>(context => this.DownloadFeedAsync(context), new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Environment.ProcessorCount });

        foreach (ContentItem contentItem in feed)
        {
            foreach (ContentAttachment attachment in contentItem.Content.Attachments)
            {
                var context = new DataflowContext { Source = attachment.Url, Destination = Path.GetFullPath(Path.Join(outputPath, attachment.Path)) };

                downloadFeedBlock.Post(context);
            }
        }

        downloadFeedBlock.Complete();

        await downloadFeedBlock.Completion.ConfigureAwait(false);

        AnsiConsole.WriteLine("File Download Completed");
    }

    private async Task<DataflowContext> DownloadFeedAsync(DataflowContext context)
    {
        try
        {
            await Retriable.RetryAsync(
                async () =>
                {
                    if (File.Exists(context.Destination))
                    {
                        context.AlreadyDownloaded = true;

                        AnsiConsole.WriteLine("Already Downloaded: " + context.Destination);

                        return;
                    }
                    else
                    {
                        var fileInfo = new FileInfo(context.Destination);

                        if (!fileInfo.Directory.Exists)
                        {
                            fileInfo.Directory.Create();
                        }
                    }

                    using (HttpClient client = this.httpClientFactory.CreateClient())
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Source);

                        HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

                        using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            using (Stream streamToWriteTo = File.Open(context.Destination, FileMode.Create))
                            {
                                await streamToReadFrom.CopyToAsync(streamToWriteTo).ConfigureAwait(false);
                            }
                        }

                        AnsiConsole.WriteLine("Downloaded: " + context.Destination);

                        response.EnsureSuccessStatusCode();
                    }
                },
                CancellationToken.None,
                new Backoff(5, TimeSpan.FromSeconds(1)),
                new AnyExceptionPolicy()).ConfigureAwait(false);
            context.IsFaulted = false;
        }
        catch (Exception ex)
        {
            context.IsFaulted = true;
            context.FaultError = ex.Message;

            AnsiConsole.WriteLine("Error Downloading: " + context.Destination);
        }

        return context;
    }
}