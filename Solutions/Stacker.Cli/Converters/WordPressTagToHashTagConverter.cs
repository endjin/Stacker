// <copyright file="WordPressTagToHashTagConverter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Globalization;

namespace Stacker.Cli.Converters;

public class WordPressTagToHashTagConverter
{
    public string Convert(string wordpressTag)
    {
        TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;

        string formatted = textInfo.ToTitleCase(wordpressTag.Replace("-", " ")).Replace(" ", string.Empty);

        if (formatted.Length == 2)
        {
            formatted = formatted.ToUpperInvariant();
        }

        return formatted;
    }
}