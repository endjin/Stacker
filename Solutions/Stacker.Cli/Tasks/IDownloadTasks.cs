// <copyright file="IDownloadTasks.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

using Stacker.Cli.Domain.Universal;

namespace Stacker.Cli.Tasks;

public interface IDownloadTasks
{
    Task DownloadAsync(List<ContentItem> feed, string outputPath);
}