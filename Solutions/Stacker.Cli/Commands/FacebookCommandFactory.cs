// <copyright file="FacebookCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.CommandLine;
using Stacker.Cli.Contracts.Commands;

namespace Stacker.Cli.Commands;

public class FacebookCommandFactory : ICommandFactory<FacebookCommandFactory>
{
    private readonly ICommandFactory<FacebookBufferCommandFactory> facebookBufferCommandFactory;

    public FacebookCommandFactory(ICommandFactory<FacebookBufferCommandFactory> facebookBufferCommandFactory)
    {
        this.facebookBufferCommandFactory = facebookBufferCommandFactory;
    }

    public Command Create()
    {
        var cmd = new Command("facebook", "Facebook functionality.");

        cmd.AddCommand(this.facebookBufferCommandFactory.Create());

        return cmd;
    }
}