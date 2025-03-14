// <copyright file="LinkedInBufferShuffleCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Stacker.Cli.Contracts.Tasks;

namespace Stacker.Cli.Commands;

/// <summary>
/// Command to shuffle the Buffer queue for a LinkedIn profile.
/// </summary>
public class LinkedInBufferShuffleCommand : BufferShuffleCommand
{
    public LinkedInBufferShuffleCommand(IContentTasks contentTasks)
        : base(contentTasks, "linkedin")
    {
    }
}