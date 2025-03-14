// <copyright file="IBufferClient.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stacker.Cli.Contracts.Buffer;

public interface IBufferClient
{
    Task UploadAsync(IEnumerable<string> content, string profileId, bool whatIf);

    Task<BufferShuffleResponse> ShuffleAsync(string profileId, int? count = null, bool? utc = null);
}