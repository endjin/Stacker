// <copyright file="ReplaceNewLineWithParagraphTagCleaner.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Stacker.Cli.Domain.Universal;

namespace Stacker.Cli.Cleaners;

public class ReplaceNewLineWithParagraphTagCleaner : IPreDownloadCleaner
{
    public ContentItem Clean(ContentItem contentItem)
    {
        contentItem.Content.Body = contentItem.Content.Body.Replace("\n", "<p/>");

        return contentItem;
    }
}