// <copyright file="FacebookCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands.Facebook
{
    using System.CommandLine;
    using Stacker.Cli.Commands.Facebook.Buffer;
    using Stacker.Cli.Commands.LinkedIn.Buffer;

    public class FacebookCommandFactory : ICommandFactory<FacebookCommandFactory>
    {
        private readonly ICommandFactory<FacebookBufferCommandFactory> facebookBufferCommandFactory;

        public FacebookCommandFactory(ICommandFactory<FacebookBufferCommandFactory> facebookBufferCommandFactory)
        {
            this.facebookBufferCommandFactory = facebookBufferCommandFactory;
        }

        public Command Create()
        {
            var cmd = new Command("facebook", "Facebook functionality.");

            cmd.AddCommand(this.facebookBufferCommandFactory.Create());

            return cmd;
        }
    }
}