// <copyright file="IPreDownloadCleaner.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Cleaners
{
    using Stacker.Cli.Domain.Universal;

    public interface IPreDownloadCleaner
    {
        ContentItem Clean(ContentItem contentItem);
    }
}
