// <copyright file="PostExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.WordPress
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Stacker.Cli.Configuration;

    public static class PostExtensions
    {
        public static IEnumerable<Post> FilterByPromotable(this IEnumerable<Post> posts)
        {
            return posts.Where(p => (p.Promote && p.PromoteUntil == DateTimeOffset.MinValue) || (p.Promote && p.PromoteUntil > DateTimeOffset.Now));
        }

        public static IEnumerable<Post> FilterByValid(this IEnumerable<Post> posts, StackerSettings settings)
        {
            return posts.Where(p => settings.Users.Exists(u => u.IsActive && string.Equals(u.Email, p.Author.Email, StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}