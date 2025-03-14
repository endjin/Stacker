// <copyright file="MastodonBufferShuffleCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Stacker.Cli.Contracts.Tasks;

namespace Stacker.Cli.Commands;

/// <summary>
/// Command to shuffle the Buffer queue for a Mastodon profile.
/// </summary>
public class MastodonBufferShuffleCommand : BufferShuffleCommand
{
    public MastodonBufferShuffleCommand(IContentTasks contentTasks)
        : base(contentTasks, "mastodon")
    {
    }
}