// <copyright file="Posting.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.LinkedIn
{
    using System.Collections.Generic;

    public class Posting
    {
        public string Author { get; set; }

        public string Body { get; set; }

        public Image Image { get; set; }

        public string Link { get; set; }

        public IEnumerable<string> Tags { get; internal set; }
    }
}