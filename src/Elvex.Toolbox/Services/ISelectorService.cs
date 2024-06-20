// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISelectorService<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets the value based on configured data
        /// </summary>
        TValue? GetValue(in TKey key);
    }
}
