// <copyright file="ContentDetails.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.Universal
{
    using System.Collections.Generic;

    public class ContentDetails
    {
        public List<ContentAttachment> Attachments { get; internal set; } = new List<ContentAttachment>();

        public string Body { get; set; }

        public string Excerpt { get; set; }

        public string Link { get; set; }

        public string Title { get; set; }
    }
}