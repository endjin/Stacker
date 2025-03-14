﻿// <copyright file="EnvironmentInitCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

#nullable enable annotations

using System.Collections.Generic;

using Spectre.Console;
using Spectre.Console.Cli;

using Stacker.Cli.Configuration;
using Stacker.Cli.Contracts.Configuration;

namespace Stacker.Cli.Commands;

public class EnvironmentInitCommand : Command
{
    private readonly IAppEnvironment appEnvironment;
    private readonly IStackerSettingsManager settingsManager;

    public EnvironmentInitCommand(IAppEnvironment appEnvironment, IStackerSettingsManager settingsManager)
    {
        this.appEnvironment = appEnvironment;
        this.settingsManager = settingsManager;
    }

    public override int Execute(CommandContext context)
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
                Users =
                [
                    new User
                    {
                        Email = string.Empty,
                        IsActive = true,
                    }
                ],
            },
            nameof(StackerSettings));

        AnsiConsole.WriteLine($"Environment Initialized {this.appEnvironment.AppPath}");

        return 0;
    }
}