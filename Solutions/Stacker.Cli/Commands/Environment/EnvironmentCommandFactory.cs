// <copyright file="EnvironmentCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands.Environment
{
    using System.CommandLine;
    using Stacker.Cli.Commands.Environment.Init;

    public class EnvironmentCommandFactory : ICommandFactory<EnvironmentCommandFactory>
    {
        private readonly ICommandFactory<EnvironmentInitCommandFactory> environmentResetCommandFactory;

        public EnvironmentCommandFactory(ICommandFactory<EnvironmentInitCommandFactory> environmentResetCommandFactory)
        {
            this.environmentResetCommandFactory = environmentResetCommandFactory;
        }

        public Command Create()
        {
            var cmd = new Command("environment", "Manipulate the stacker environment.");

            cmd.AddCommand(this.environmentResetCommandFactory.Create());

            return cmd;
        }
    }
}