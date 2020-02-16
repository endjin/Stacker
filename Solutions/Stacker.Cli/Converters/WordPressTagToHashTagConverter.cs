// <copyright file="WordPressTagToHashTagConverter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Converters
{
    using System.Globalization;

    public class WordPressTagToHashTagConverter
    {
        public string Convert(string wordpressTag)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;

            var formatted = textInfo.ToTitleCase(wordpressTag.Replace("-", " ")).Replace(" ", string.Empty);

            if (formatted.Length == 2)
            {
                formatted = formatted.ToUpperInvariant();
            }

            return formatted;
        }
    }
}