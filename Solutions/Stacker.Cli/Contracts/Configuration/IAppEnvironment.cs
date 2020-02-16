// <copyright file="IAppEnvironment.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Contracts.Configuration
{
    public interface IAppEnvironment
    {
        string AppPath { get; }

        string ConfigurationPath { get; }

        void Clean();

        void Initialize();

        bool IsInitialized();
    }
}