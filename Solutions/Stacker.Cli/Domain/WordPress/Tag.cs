// <copyright file="Tag.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.WordPress
{
    using System.Diagnostics;

    [DebuggerDisplay("{Slug} Id = {Id}")]
    public class Tag
    {
        public string Id { get; set; }

        public string Slug { get; set; }
    }
}