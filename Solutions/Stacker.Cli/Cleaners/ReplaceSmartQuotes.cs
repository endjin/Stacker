// <copyright file="ReplaceSmartQuotes.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Cleaners;

public class ReplaceSmartQuotes : IPostConvertCleaner
{
    public string Clean(string content)
    {
        return content.Replace("“", "\"").Replace("”", "\"").Replace("’", "'").Replace("‘", "'");
    }
}