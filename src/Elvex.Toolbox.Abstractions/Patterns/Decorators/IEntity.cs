// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Patterns.Decorators
{
    /// <summary>
    /// Define an entity with a unique identifier
    /// </summary>
    public interface IEntity<out TUid>
        where TUid : IEquatable<TUid>
    {
        /// <summary>
        /// Gets the uid.
        /// </summary>
        TUid Uid { get; }   
    }
}
