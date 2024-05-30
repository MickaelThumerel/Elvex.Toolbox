﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

// KEEP : Microsoft.Extensions.DependencyInjection
namespace Microsoft.Extensions.DependencyInjection
{
    using System.Collections.Generic;

    /// <summary>
    /// Dictionary that represent all the key services registred
    /// </summary>
    public interface IKeyServicesReadOnlyDictionary<TKey, TService> : IReadOnlyDictionary<TKey, TService>   
    {
    }
}
