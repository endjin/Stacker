// <copyright file="FacebookBufferShuffleCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Stacker.Cli.Contracts.Tasks;

namespace Stacker.Cli.Commands;

/// <summary>
/// Command to shuffle the Buffer queue for a Facebook profile.
/// </summary>
public class FacebookBufferShuffleCommand : BufferShuffleCommand
{
    public FacebookBufferShuffleCommand(IContentTasks contentTasks)
        : base(contentTasks, "facebook")
    {
    }
}