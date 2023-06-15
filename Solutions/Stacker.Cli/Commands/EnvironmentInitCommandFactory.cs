// <copyright file="EnvironmentInitCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using Stacker.Cli.Configuration;
using Stacker.Cli.Contracts.Commands;
using Stacker.Cli.Contracts.Configuration;

namespace Stacker.Cli.Commands;

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
                this.settingsManager.SaveSettings(
                    new StackerSettings
                    {
                        BufferAccessToken = "<ADD YOUR ACCESS TOKEN>",
                        BufferProfiles = new Dictionary<string, string>
                        {
                            { "facebook|<ACCOUNT NAME>", "<BUFFER CHANNEL ID>" },
                            { "linkedin|<ACCOUNT NAME>", "<BUFFER CHANNEL ID>" },
                            { "twitter|<ACCOUNT NAME>", "<BUFFER CHANNEL ID>" },
                        },
                        Users = new List<User>
                        {
                            new User
                            {
                                Email = string.Empty,
                                IsActive = true,
                            },
                        },
                    },
                    nameof(StackerSettings));

                Console.WriteLine($"Environment Initialized {this.appEnvironment.AppPath}");
            }),
        };
    }
}