// <copyright file="EnvironmentInitCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

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

    public override int Execute([NotNull] CommandContext context, CancellationToken cancellationToken)
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
                        Twitter = string.Empty,
                    }
                ],
                WordPressToMarkdown = new WordPressToMarkdown(),
            },
            nameof(StackerSettings));

        AnsiConsole.WriteLine($"Environment Initialized {this.appEnvironment.AppPath}");

        return 0;
    }
}