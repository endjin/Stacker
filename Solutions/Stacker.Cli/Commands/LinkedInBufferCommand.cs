// <copyright file="LinkedInBufferCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

#nullable enable annotations
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Spectre.Console.Cli;
using Spectre.IO;

using Stacker.Cli.Contracts.Tasks;
using Stacker.Cli.Domain.Publication;
using Stacker.Cli.Formatters;

namespace Stacker.Cli.Commands;

public class LinkedInBufferCommand : AsyncCommand<LinkedInBufferCommand.Settings>
{
    private readonly IContentTasks contentTasks;
    private readonly string profilePrefix = "linkedin|";

    public LinkedInBufferCommand(IContentTasks contentTasks)
    {
        this.contentTasks = contentTasks;
    }

    /// <inheritdoc/>
    public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        await this.contentTasks.BufferContentItemsAsync<FacebookFormatter>(
            settings.ContentFilePath,
            this.profilePrefix,
            settings.ProfileName,
            settings.PublicationPeriod,
            settings.FromDate,
            settings.ToDate,
            settings.ItemCount,
            settings.FilterByTag,
            settings.WhatIf).ConfigureAwait(false);

        return 0;
    }

    /// <summary>
    /// The settings for the command.
    /// </summary>
    public class Settings : CommandSettings
    {
#nullable disable annotations

        [CommandOption("-c|--content-file-path <ContentFilePath>")]
        [Description("Content file path.")]
        public FilePath ContentFilePath { get; init; }

        [CommandOption("-n|--profile-name <ProfileName>")]
        [Description("LinkedIn profile to Buffer.")]
        public string ProfileName { get; init; }

        [CommandOption("-g|--filter-by-tag <FilterByTag>")]
        [Description("Tag to filter the content items by.")]
        public string FilterByTag { get; init; }

        [CommandOption("-i|--item-count <ItemCount>")]
        [Description("Number of content items to buffer. If omitted all content is buffered.")]
        public int ItemCount { get; init; }

        [CommandOption("-p|--publication-period <PublicationPeriod>")]
        [Description("Publication period to filter content items by. <LastMonth|LastWeek|LastYear|None|ThisMonth|ThisWeek|ThisYear> If specified --from-date and --to-date are ignored.")]
        public PublicationPeriod PublicationPeriod { get; init; }

        [CommandOption("-f|--from-date <FromDate>")]
        [Description("Include content items published on, or after this date. Use YYYY/MM/DD Format. If omitted DateTime.MinValue is used.")]
        public DateTime FromDate { get; init; }

        [CommandOption("-t|--to-date <ToDate>")]
        [Description("Include content items published on, or before this date. Use YYYY/MM/DD Format. If omitted DateTime.MaxValue is used.")]
        public DateTime ToDate { get; init; }

        [CommandOption("-w|--what-if <WhatIf>")]
        [Description("See what the command would do without submitting the content to Buffer.")]
        public bool WhatIf { get; set; }

#nullable enable annotations
    }
}