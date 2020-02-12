// <copyright file="Image.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.LinkedIn
{
    using System.Diagnostics;

    [DebuggerDisplay("{Title}")]
    public class Image
    {
        public string Title { get; set; }

        public string Url { get; set; }
    }
}