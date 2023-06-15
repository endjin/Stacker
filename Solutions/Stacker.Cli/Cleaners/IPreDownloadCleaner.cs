// <copyright file="IPreDownloadCleaner.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Stacker.Cli.Domain.Universal;

namespace Stacker.Cli.Cleaners;

public interface IPreDownloadCleaner
{
    ContentItem Clean(ContentItem contentItem);
}