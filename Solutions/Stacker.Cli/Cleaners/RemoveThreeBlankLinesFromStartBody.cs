// <copyright file="RemoveThreeBlankLinesFromStartBody.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Text.RegularExpressions;

namespace Stacker.Cli.Cleaners;

public class RemoveThreeBlankLinesFromStartBody : IPostConvertCleaner
{
    public string Clean(string content)
    {
        Regex regexp = new Regex(@"(\r\n){3,3}", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

        return regexp.Replace(content, Environment.NewLine);
    }
}