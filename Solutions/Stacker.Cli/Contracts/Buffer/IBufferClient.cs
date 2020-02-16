// <copyright file="IBufferClient.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Contracts.Buffer
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBufferClient
    {
        Task UploadAsync(IEnumerable<string> content, string profileId);
    }
}