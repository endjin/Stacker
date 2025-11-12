// <copyright file="SettingsManager{T}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text.Json;

using Stacker.Cli.Contracts.Configuration;

namespace Stacker.Cli.Configuration;

public class SettingsManager<T> : ISettingsManager<T>
    where T : class
{
    private readonly IAppEnvironment appEnvironment;

    public SettingsManager(IAppEnvironment appEnvironment)
    {
        this.appEnvironment = appEnvironment;
    }

    public T LoadSettings(string fileName)
    {
        string filePath = $"{this.GetLocalFilePath(fileName)}.json";

        return File.Exists(filePath)
            ? JsonSerializer.Deserialize<T>(File.ReadAllText(filePath)) ?? throw new InvalidOperationException($"Failed to deserialize settings from {filePath}")
            : throw new FileNotFoundException($"Settings file not found: {filePath}");
    }

    public void SaveSettings(T settings, string fileName)
    {
        string filePath = this.GetLocalFilePath(fileName);
        string json = JsonSerializer.Serialize(settings);

        File.WriteAllText($"{filePath}.json", json);
    }

    private string GetLocalFilePath(string fileName)
    {
        return Path.Combine(this.appEnvironment.ConfigurationPath, fileName);
    }
}