// <copyright file="DataflowContext.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Tasks;

public class DataflowContext
{
    public string Destination { get; set; } = string.Empty;

    public string Source { get; set; } = string.Empty;

    public bool IsFaulted { get; set; }

    public bool AlreadyDownloaded { get; set; }

    public string FaultError { get; set; } = string.Empty;
}