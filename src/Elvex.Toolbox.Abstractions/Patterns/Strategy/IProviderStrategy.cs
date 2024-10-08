﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Patterns.Strategy
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Define a provider through strategy pattern to give access to <typeparamref name="T"/> using <see cref="IProviderStrategySource{T, TKey}"/>
    /// </summary>
    public interface IProviderStrategy<T, TKey>
        where T : class
    {
        #region Events

        /// <summary>
        /// Occurs when source have been update.
        /// </summary>
        event EventHandler<IReadOnlyCollection<TKey>>? DataChanged;

        #endregion

        #region Methods

        /// <summary>
        /// Gets all values corresponding to <paramref name="keys"/>
        /// </summary>
        ValueTask<IReadOnlyCollection<T>> GetAllValuesAsync(CancellationToken token);
        
        /// <summary>
        /// Gets all values corresponding match conditions
        /// </summary>
        ValueTask<IReadOnlyCollection<T>> GetValuesAsync(Expression<Func<T, bool>> filter, CancellationToken token);

        /// <summary>
        /// Gets all values corresponding to <paramref name="keys"/>
        /// </summary>
        ValueTask<IReadOnlyCollection<T>> GetByKeyAsync(CancellationToken token, params TKey[] keys);

        /// <summary>
        /// Gets all values corresponding to <paramref name="keys"/>
        /// </summary>
        ValueTask<IReadOnlyCollection<T>> GetByKeyAsync(IReadOnlyCollection<TKey> keys, CancellationToken token);

        /// <summary>
        /// Gets all keys corresponding match conditions
        /// </summary>
        ValueTask<IReadOnlyCollection<TKey>> GetKeysAsync(Expression<Func<T, bool>> filter, CancellationToken token);

        /// <summary>
        /// Gets first value to correspond to <paramref name="key"/>
        /// </summary>
        ValueTask<T?> GetByKeyAsync(TKey key, CancellationToken token);

        /// <summary>
        /// Try get first value to correspond to <paramref name="key"/>
        /// </summary>
        ValueTask<(bool Result, T? value)> TryGetByKeyAsync(TKey key, CancellationToken token);

        /// <summary>
        /// Gets first value to correspond match conditions
        /// </summary>
        ValueTask<T?> GetFirstValueAsync(Expression<Func<T, bool>> filter, CancellationToken token);

        /// <summary>
        /// Forces cache data to update
        /// </summary>
        ValueTask ForceUpdateAsync(CancellationToken token);

        #endregion
    }
}
