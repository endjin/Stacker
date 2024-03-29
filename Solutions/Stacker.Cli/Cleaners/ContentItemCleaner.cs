﻿// <copyright file="ContentItemCleaner.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using Stacker.Cli.Domain.Universal;

namespace Stacker.Cli.Cleaners;

public class ContentItemCleaner
{
    private readonly IServiceProvider serviceProvider;

    public ContentItemCleaner(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public ContentItem PreDownload(ContentItem content)
    {
        IEnumerable<IPreDownloadCleaner> cleaners = this.serviceProvider.GetServices<IPreDownloadCleaner>();
        return cleaners.Aggregate(content, (current, cleaner) => cleaner.Clean(current));
    }

    public ContentItem PostDownload(ContentItem content)
    {
        IEnumerable<IPostDownloadCleaner> cleaners = this.serviceProvider.GetServices<IPostDownloadCleaner>();
        return cleaners.Aggregate(content, (current, cleaner) => cleaner.Clean(current));
    }

    internal string PostConvert(string content)
    {
        IEnumerable<IPostConvertCleaner> cleaners = this.serviceProvider.GetServices<IPostConvertCleaner>();
        return cleaners.Aggregate(content, (current, cleaner) => cleaner.Clean(current));
    }
}