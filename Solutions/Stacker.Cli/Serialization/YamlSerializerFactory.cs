// <copyright file="YamlSerializerFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using YamlDotNet.Serialization;

namespace Stacker.Cli.Serialization;

public class YamlSerializerFactory : IYamlSerializerFactory
{
    public ISerializer GetSerializer()
    {
        return new SerializerBuilder().WithEventEmitter(next => new ForceQuotedStringValuesEventEmitter(next)).Build();
    }
}