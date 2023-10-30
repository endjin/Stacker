// <copyright file="TagAliases.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Stacker.Cli.Configuration;

public class TagAliases
{
    public string Tag { get; set; }

    public List<string> Aliases { get; set; }
}