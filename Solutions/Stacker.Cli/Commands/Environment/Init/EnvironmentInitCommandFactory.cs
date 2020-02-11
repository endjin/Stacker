// <copyright file="EnvironmentInitCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands.Environment.Init
{
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using Stacker.Cli.Configuration;
    using Stacker.Cli.Configuration.Contracts;

    public class EnvironmentInitCommandFactory : ICommandFactory<EnvironmentInitCommandFactory>
    {
        private readonly IAppEnvironment appEnvironment;
        private readonly IStackerSettingsManager settingsManager;

        public EnvironmentInitCommandFactory(IAppEnvironment appEnvironment, IStackerSettingsManager settingsManager)
        {
            this.appEnvironment = appEnvironment;
            this.settingsManager = settingsManager;
        }

        public Command Create()
        {
            return new Command("init", "Initializes the stacker environment.")
            {
                Handler = CommandHandler.Create(() =>
                {
                    this.appEnvironment.Initialize();
                    this.settingsManager.SaveSettings(new StackerSettings { Users = new List<User> { new User { Email = string.Empty, IsActive = true } } }, nameof(StackerSettings));
                    Console.WriteLine($"Environment Initialized {this.appEnvironment.AppPath}");
                }),
            };
        }
    }
}