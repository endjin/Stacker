// <copyright file="TwitterFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Stacker.Cli.Contracts.Formatters;

namespace Stacker.Cli.Formatters;

public class TwitterFormatter : ShortFormContentFormatter, IContentFormatter
{
    private const int MaxContentLength = 280;

    public TwitterFormatter()
        : base(MaxContentLength, "twitter")
    {
    }
}