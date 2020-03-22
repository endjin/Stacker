// <copyright file="Attachment.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.WordPress
{
    using System.Diagnostics;

    [DebuggerDisplay("{Title} Id = {Id}")]
    public class Attachment
    {
        public string Id { get; set; }

        public string Path { get; internal set; }

        public string PostId { get; internal set; }

        public string Title { get; set; }

        public string Url { get; set; }
    }
}