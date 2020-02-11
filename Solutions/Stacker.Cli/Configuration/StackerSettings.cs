// <copyright file="StackerSettings.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Configuration
{
    using System.Collections.Generic;

    public class StackerSettings
    {
        /// <summary>
        /// Gets or sets the selected ADR template name.
        /// </summary>
        public List<User> Users { get; set; } = new List<User>();
    }
}