// <copyright file="FacebookFormatter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Formatters
{
    using Stacker.Cli.Contracts.Formatters;

    public class FacebookFormatter : LongFormContentFormatter, IContentFormatter
    {
        private const int MaxContentLength = 63_206;

        public FacebookFormatter()
            : base(MaxContentLength, "facebook")
        {
        }
    }
}