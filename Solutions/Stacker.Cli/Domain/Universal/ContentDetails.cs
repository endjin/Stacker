// <copyright file="ContentDetails.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.Universal
{
    using System.Collections.Generic;
    using System.Linq;

    public class ContentDetails
    {
        public ContentDetails()
        {
            this.Attachments = Enumerable.Empty<ContentAttachment>();
        }

        public IEnumerable<ContentAttachment> Attachments { get; internal set; }

        public string Body { get; set; }

        public string Excerpt { get; set; }

        public string Link { get; set; }

        public string Title { get; set; }
    }
}