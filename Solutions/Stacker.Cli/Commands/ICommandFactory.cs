// <copyright file="ICommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands
{
    using System.CommandLine;

    public interface ICommandFactory<T>
    {
        Command Create();
    }
}