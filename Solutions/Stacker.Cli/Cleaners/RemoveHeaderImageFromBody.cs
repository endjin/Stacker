﻿// <copyright file="RemoveHeaderImageFromBody.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Text.RegularExpressions;

namespace Stacker.Cli.Cleaners;

public class RemoveHeaderImageFromBody : IPostConvertCleaner
{
    public string Clean(string content)
    {
        Regex regexp = new(@"(\[?!\[.*\]\(.*\))", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

        return regexp.Replace(content, string.Empty, 1, 0);
    }
}