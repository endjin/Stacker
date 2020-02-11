// <copyright file="User.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Configuration
{
    using System.Diagnostics;

    [DebuggerDisplay("{Email} IsActive = {IsActive}")]
    public class User
    {
        public string Email { get; set; }

        public bool IsActive { get; set; }

        public string Twitter { get; set; }
    }
}