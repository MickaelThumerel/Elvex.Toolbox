﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Extensions.Types
{
    using System.Threading.Tasks;

    /// <summary>
    /// Enhance type <see cref="ValueTask"/> and <see cref="ValueTask{TResult}"/>
    /// </summary>
    /// <seealso cref="ITypeInfoExtensionEnhancer" />
    public interface IValueTaskTypeInfoEnhancer : ITypeInfoExtensionEnhancer
    {
        /// <summary>
        /// return task from <see cref="ValueTask"/> or <see cref="ValueTask{TResult}"/>
        /// </summary>
        Task AsTask(object? valueTask);

        /// <summary>
        /// Gets the <see cref="ValueTask.FromResult"/> from result.
        /// </summary>
        object? GetValueTaskFromResult(object? resultInst);
    }
}
