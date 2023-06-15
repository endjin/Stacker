// <copyright file="IPostDownloadCleaner.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Stacker.Cli.Domain.Universal;

namespace Stacker.Cli.Cleaners;

public interface IPostDownloadCleaner
{
    ContentItem Clean(ContentItem contentItem);
}