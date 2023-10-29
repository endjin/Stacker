// <copyright file="IContentTasks.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Spectre.IO;

using Stacker.Cli.Contracts.Formatters;
using Stacker.Cli.Domain.Publication;
using Stacker.Cli.Domain.Universal;

namespace Stacker.Cli.Contracts.Tasks;

public interface IContentTasks
{
    Task BufferContentItemsAsync<TContentFormatter>(
        FilePath contentFilePath,
        string profilePrefix,
        string profileName,
        PublicationPeriod publicationPeriod,
        DateTime fromDate,
        DateTime toDate,
        int itemCount,
        string filterByTag)
        where TContentFormatter : class, IContentFormatter, new();

    Task<IEnumerable<ContentItem>> LoadContentItemsAsync(
        FilePath contentFilePath,
        PublicationPeriod publicationPeriod,
        DateTime fromDate,
        DateTime toDate,
        int itemCount,
        string filterByTag);
}