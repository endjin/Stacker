// <copyright file="BufferShuffleCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Spectre.Console.Cli;
using Stacker.Cli.Contracts.Buffer;
using Stacker.Cli.Contracts.Tasks;

namespace Stacker.Cli.Commands;

[Description("Shuffles the Buffer queue for the specified profile")]
public class BufferShuffleCommand : AsyncCommand<BufferShuffleCommand.Settings>
{
    private readonly IContentTasks contentTasks;
    private readonly string profilePrefix = "buffer|";

    public BufferShuffleCommand(IContentTasks contentTasks)
    {
        this.contentTasks = contentTasks;
    }

    /// <inheritdoc/>
    public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        BufferShuffleResponse result = await this.contentTasks.ShuffleBufferQueueAsync(
            this.profilePrefix,
            settings.ProfileName,
            settings.Count).ConfigureAwait(false);

        return result.Success ? 0 : 1;
    }

    /// <summary>
    /// The settings for the command.
    /// </summary>
    public class Settings : CommandSettings
    {
        [CommandOption("-n|--profile-name")]
        [Description("Buffer profile name to shuffle.")]
        public string ProfileName { get; init; }

        [CommandOption("-c|--count")]
        [Description("Number of updates to shuffle. If omitted, all scheduled updates are shuffled.")]
        public int? Count { get; init; }
    }
}