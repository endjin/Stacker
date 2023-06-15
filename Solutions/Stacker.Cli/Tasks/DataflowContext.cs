// <copyright file="DataflowContext.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Tasks;

public class DataflowContext
{
    public string Destination { get; set; }

    public string Source { get; set; }

    public bool IsFaulted { get; set; }

    public bool AlreadyDownloaded { get; set; }

    public string FaultError { get; set; }
}