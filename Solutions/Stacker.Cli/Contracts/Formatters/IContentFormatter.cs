// <copyright file="IContentFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Stacker.Cli.Domain.Universal;

namespace Stacker.Cli.Contracts.Formatters;

public interface IContentFormatter
{
    IEnumerable<string> Format(string campaignMedium, string campaignName, IEnumerable<ContentItem> feedItems);
}