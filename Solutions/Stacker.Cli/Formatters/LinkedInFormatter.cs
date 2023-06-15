// <copyright file="LinkedInFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Stacker.Cli.Contracts.Formatters;

namespace Stacker.Cli.Formatters;

public class LinkedInFormatter : LongFormContentFormatter, IContentFormatter
{
    private const int MaxContentLength = 1300;

    public LinkedInFormatter()
        : base(MaxContentLength, "linkedin")
    {
    }
}