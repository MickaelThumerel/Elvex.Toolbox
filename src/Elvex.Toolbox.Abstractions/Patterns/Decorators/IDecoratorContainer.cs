// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Patterns.Decorators
{
    /// <summary>
    /// Relation key between a source of data linked by <typeparamref name="TSourceKey"/> and a external data <typeparamref name="TData"/>
    /// </summary>
    public interface IDecoratorContainer<out TData, out TSourceKey>
    {
        /// <summary>
        /// Gets the data decorating.
        /// </summary>
        TData Data { get; }

        /// <summary>
        /// Gets the source key decorate by this data.
        /// </summary>
        TSourceKey SourceKey { get; }
    }
}
