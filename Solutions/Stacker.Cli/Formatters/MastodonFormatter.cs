// <copyright file="MastodonFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Stacker.Cli.Contracts.Formatters;

namespace Stacker.Cli.Formatters;

public class MastodonFormatter : ShortFormContentFormatter, IContentFormatter
{
    private const int MaxContentLength = 500;

    public MastodonFormatter()
        : base(MaxContentLength, "mastodon")
    {
    }
}