// <copyright file="ContentItemCleaner.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Cleaners
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Stacker.Cli.Domain.Universal;

    public class ContentItemCleaner
    {
        private readonly IServiceProvider serviceProvider;

        public ContentItemCleaner(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ContentItem PreDownload(ContentItem content)
        {
            var cleaners = this.serviceProvider.GetServices<IPreDownloadCleaner>();
            return cleaners.Aggregate(content, (current, cleaner) => cleaner.Clean(current));
        }

        public ContentItem PostDownload(ContentItem content)
        {
            var cleaners = this.serviceProvider.GetServices<IPostDownloadCleaner>();
            return cleaners.Aggregate(content, (current, cleaner) => cleaner.Clean(current));
        }

        internal string PostConvert(string content)
        {
            var cleaners = this.serviceProvider.GetServices<IPostConvertCleaner>();
            return cleaners.Aggregate(content, (current, cleaner) => cleaner.Clean(current));
        }
    }
}
