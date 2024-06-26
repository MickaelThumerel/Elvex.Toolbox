﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Disposables
{
    using System;

    /// <summary>
    /// Define a thread safe disposable instance.
    /// </summary>
    public interface ISafeAsyncDisposable : IAsyncDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        bool IsDisposed { get; }
    }

    /// <summary>
    /// Define a disposable instance thread safe that transport a <typeparamref name="TContent"/>
    /// </summary>
    public interface ISafeAsyncDisposable<TContent> : ISafeAsyncDisposable
    {
        /// <summary>
        /// Gets the content.
        /// </summary>
        public TContent Content { get; }
    }
}
