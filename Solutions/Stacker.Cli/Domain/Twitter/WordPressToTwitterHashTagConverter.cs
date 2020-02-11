// <copyright file="WordPressToTwitterHashTagConverter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.Twitter
{
    using System.Globalization;

    public class WordPressToTwitterHashTagConverter
    {
        public string Convert(string wordpressHash)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;

            return textInfo.ToTitleCase(wordpressHash.Replace("-", " ")).Replace(" ", string.Empty);
        }
    }
}