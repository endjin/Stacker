// <copyright file="TwitterCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands.Twitter
{
    using System.CommandLine;
    using Stacker.Cli.Commands.Twitter.Buffer;

    public class TwitterCommandFactory : ICommandFactory<TwitterCommandFactory>
    {
        private readonly ICommandFactory<TwitterBufferCommandFactory> twitterBufferCommandFactory;

        public TwitterCommandFactory(ICommandFactory<TwitterBufferCommandFactory> twitterBufferCommandFactory)
        {
            this.twitterBufferCommandFactory = twitterBufferCommandFactory;
        }

        public Command Create()
        {
            var cmd = new Command("twitter", "Twitter functionality.");

            cmd.AddCommand(this.twitterBufferCommandFactory.Create());

            return cmd;
        }
    }
}