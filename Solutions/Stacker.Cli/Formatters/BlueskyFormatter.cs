// <copyright file="BlueskyFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Stacker.Cli.Contracts.Formatters;

namespace Stacker.Cli.Formatters;

public class BlueskyFormatter : ShortFormContentFormatter, IContentFormatter
{
    private const int MaxContentLength = 300;

    public BlueskyFormatter()
        : base(MaxContentLength, "bluesky")
    {
    }
}