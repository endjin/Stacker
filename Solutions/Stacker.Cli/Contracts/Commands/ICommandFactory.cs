// <copyright file="ICommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.CommandLine;

namespace Stacker.Cli.Contracts.Commands;

public interface ICommandFactory<T>
{
    Command Create();
}