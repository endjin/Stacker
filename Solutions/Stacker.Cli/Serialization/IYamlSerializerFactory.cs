// <copyright file="IYamlSerializerFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Serialization
{
    using YamlDotNet.Serialization;

    public interface IYamlSerializerFactory
    {
        ISerializer GetSerializer();
    }
}
