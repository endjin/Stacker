// <copyright file="HashTag.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.Universal;

public record HashTag(string Text, string Tag, bool Default = false);