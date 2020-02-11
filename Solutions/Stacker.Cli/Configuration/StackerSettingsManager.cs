// <copyright file="StackerSettingsManager.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Configuration
{
    using Stacker.Cli.Configuration.Contracts;

    public class StackerSettingsManager : SettingsManager<StackerSettings>, IStackerSettingsManager
    {
        public StackerSettingsManager(IAppEnvironment appEnvironment)
            : base(appEnvironment)
        {
        }
    }
}