// <copyright file="YamlSerializerFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Serialization
{
    using YamlDotNet.Serialization;

    public class YamlSerializerFactory : IYamlSerializerFactory
    {
        public ISerializer GetSerializer()
        {
            return new SerializerBuilder().WithEventEmitter(next => new ForceQuotedStringValuesEventEmitter(next)).Build();
        }
    }
}
