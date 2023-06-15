// <copyright file="TwitterCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.CommandLine;
using Stacker.Cli.Contracts.Commands;

namespace Stacker.Cli.Commands;

public class TwitterCommandFactory : ICommandFactory<TwitterCommandFactory>
{
    private readonly ICommandFactory<TwitterBufferCommandFactory> twitterBufferCommandFactory;

    public TwitterCommandFactory(ICommandFactory<TwitterBufferCommandFactory> twitterBufferCommandFactory)
    {
        this.twitterBufferCommandFactory = twitterBufferCommandFactory;
    }

    public Command Create()
    {
        var cmd = new Command("twitter", "Twitter functionality.");

        cmd.AddCommand(this.twitterBufferCommandFactory.Create());

        return cmd;
    }
}