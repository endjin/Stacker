// <copyright file="IYamlSerializerFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using YamlDotNet.Serialization;

namespace Stacker.Cli.Serialization;

public interface IYamlSerializerFactory
{
    ISerializer GetSerializer();
}