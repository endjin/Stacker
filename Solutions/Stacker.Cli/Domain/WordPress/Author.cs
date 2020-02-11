// <copyright file="Author.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.WordPress
{
    using System.Diagnostics;

    [DebuggerDisplay("{Email} Id = {Id}")]
    public class Author
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }
    }
}