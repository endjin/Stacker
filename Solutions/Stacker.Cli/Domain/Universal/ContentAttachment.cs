// <copyright file="ContentAttachment.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.Universal;

public class ContentAttachment
{
    public required string Url { get; set; }

    public required string Path { get; set; }
}