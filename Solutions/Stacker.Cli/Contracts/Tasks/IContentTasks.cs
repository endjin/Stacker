// <copyright file="IContentTasks.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Contracts.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Stacker.Cli.Contracts.Formatters;
    using Stacker.Cli.Domain.Publication;
    using Stacker.Cli.Domain.Universal;

    public interface IContentTasks
    {
        Task BufferContentItemsAsync<TContentFormatter>(string contentFilePath, string profileKey, PublicationPeriod publicationPeriod, DateTime fromDate, DateTime toDate, int itemCount)
            where TContentFormatter : class, IContentFormatter, new();

        Task<IEnumerable<ContentItem>> LoadContentItemsAsync(string contentFilePath, PublicationPeriod publicationPeriod, DateTime fromDate, DateTime toDate, int itemCount);
    }
}