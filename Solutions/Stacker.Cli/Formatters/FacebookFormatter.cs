// <copyright file="FacebookFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Stacker.Cli.Contracts.Formatters;

namespace Stacker.Cli.Formatters;

public class FacebookFormatter : LongFormContentFormatter, IContentFormatter
{
    private const int MaxContentLength = 63_206;

    public FacebookFormatter()
        : base(MaxContentLength, "facebook")
    {
    }
}