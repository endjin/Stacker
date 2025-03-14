// <copyright file="BlueskyBufferShuffleCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Stacker.Cli.Contracts.Tasks;

namespace Stacker.Cli.Commands;

/// <summary>
/// Command to shuffle the Buffer queue for a Bluesky profile.
/// </summary>
public class BlueskyBufferShuffleCommand : BufferShuffleCommand
{
    public BlueskyBufferShuffleCommand(IContentTasks contentTasks)
        : base(contentTasks, "bluesky")
    {
    }
}