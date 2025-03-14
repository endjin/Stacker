// <copyright file="TwitterBufferShuffleCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Stacker.Cli.Contracts.Tasks;

namespace Stacker.Cli.Commands;

/// <summary>
/// Command to shuffle the Buffer queue for a Twitter profile.
/// </summary>
public class TwitterBufferShuffleCommand : BufferShuffleCommand
{
    public TwitterBufferShuffleCommand(IContentTasks contentTasks)
        : base(contentTasks, "twitter")
    {
    }
}