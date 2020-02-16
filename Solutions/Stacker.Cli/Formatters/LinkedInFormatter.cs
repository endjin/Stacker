// <copyright file="LinkedInFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Formatters
{
    using Stacker.Cli.Contracts.Formatters;

    public class LinkedInFormatter : LongFormContentFormatter, IContentFormatter
    {
        private const int MaxContentLength = 1300;

        public LinkedInFormatter()
            : base(MaxContentLength)
        {
        }
    }
}