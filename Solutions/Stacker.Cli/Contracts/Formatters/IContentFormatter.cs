// <copyright file="IContentFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Contracts.Formatters
{
    using System.Collections.Generic;
    using Stacker.Cli.Domain.Universal;

    public interface IContentFormatter
    {
        IEnumerable<string> Format(IEnumerable<ContentItem> feedItems);
    }
}