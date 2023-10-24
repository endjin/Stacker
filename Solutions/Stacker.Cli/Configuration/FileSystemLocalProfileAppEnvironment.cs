// <copyright file="FileSystemLocalProfileAppEnvironment.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.IO;

using Stacker.Cli.Contracts.Configuration;

namespace Stacker.Cli.Configuration;

public class FileSystemLocalProfileAppEnvironment : IAppEnvironment
{
    public const string AppName = "stacker";
    public const string AppOrgName = "endjin";
    public const string ConfigurationDirectorName = "configuration";

    public string AppPath
    {
        get
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppOrgName, AppName);
        }
    }

    public string ConfigurationPath
    {
        get { return Path.Combine(this.AppPath, ConfigurationDirectorName); }
    }

    public void Clean()
    {
        Directory.Delete(this.AppPath, recursive: true);
    }

    public void Initialize()
    {
        if (!Directory.Exists(this.AppPath))
        {
            Directory.CreateDirectory(this.AppPath);
        }

        if (!Directory.Exists(this.ConfigurationPath))
        {
            Directory.CreateDirectory(this.ConfigurationPath);
        }
    }

    public bool IsInitialized()
    {
        // TODO: Better probing that a template actually exists.
        return Directory.Exists(this.ConfigurationPath);
    }
}