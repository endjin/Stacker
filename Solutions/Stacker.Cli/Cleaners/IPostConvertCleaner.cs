// <copyright file="IPostConvertCleaner.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Cleaners;

public interface IPostConvertCleaner
{
    string Clean(string content);
}