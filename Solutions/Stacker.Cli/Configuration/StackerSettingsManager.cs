// <copyright file="StackerSettingsManager.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Stacker.Cli.Contracts.Configuration;

namespace Stacker.Cli.Configuration;

public class StackerSettingsManager : SettingsManager<StackerSettings>, IStackerSettingsManager
{
    public StackerSettingsManager(IAppEnvironment appEnvironment)
        : base(appEnvironment)
    {
    }
}