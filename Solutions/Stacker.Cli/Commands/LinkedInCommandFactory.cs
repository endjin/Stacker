﻿// <copyright file="LinkedInCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands
{
    using System.CommandLine;
    using Stacker.Cli.Contracts.Commands;

    public class LinkedInCommandFactory : ICommandFactory<LinkedInCommandFactory>
    {
        private readonly ICommandFactory<LinkedInBufferCommandFactory> twitterBufferCommandFactory;

        public LinkedInCommandFactory(ICommandFactory<LinkedInBufferCommandFactory> twitterBufferCommandFactory)
        {
            this.twitterBufferCommandFactory = twitterBufferCommandFactory;
        }

        public Command Create()
        {
            var cmd = new Command("linkedin", "LinkedIn functionality.");

            cmd.AddCommand(this.twitterBufferCommandFactory.Create());

            return cmd;
        }
    }
}