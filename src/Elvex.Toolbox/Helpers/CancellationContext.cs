﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Helpers
{
    using Elvex.Toolbox.Abstractions.Disposables;
    using Elvex.Toolbox.Disposables;

    using System;

    /// <summary>
    /// Context used to simplify usage of <see cref="CancellationHelper.SingleAccessScope(SemaphoreSlim, Func{CancellationTokenSource}, Action{CancellationTokenSource}, TimeSpan?)"/>
    /// </summary>
    public sealed class CancellationContext : SafeDisposable
    {
        #region Fields

        private CancellationTokenSource? _cancellationSource;
        private readonly SemaphoreSlim _locker;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CancellationContext"/> class.
        /// </summary>
        public CancellationContext()
        {
            this._locker = new SemaphoreSlim(1);
        }

        #endregion

        #region Methods

        /// <inheritdoc cref="CancellationHelper.SingleAccessScope"/>
        public ISafeDisposable<CancellationToken> Lock(CancellationToken? token = null)
        {
            return CancellationHelper.SingleAccessScope(this._locker,
                                                        () => this._cancellationSource,
                                                        c => this._cancellationSource = c);
        }

        /// <summary>
        /// Force Cancel
        /// </summary>
        public void Cancel()
        {
            this._locker.Wait();
            try
            {
                this._cancellationSource?.Cancel();
                this._cancellationSource = null;
            }
            finally
            {
                this._locker.Release();
            }
        }

        /// <summary>
        /// Start to disposed
        /// </summary>
        protected override void DisposeBegin()
        {
            try
            {
                this._cancellationSource?.Cancel();
            }
            catch (Exception)
            {

            }

            base.DisposeBegin();
        }

        /// <summary>
        /// Call at the end of the dispose process
        /// </summary>
        protected override void DisposeEnd()
        {
            this._locker.Dispose();
            base.DisposeEnd();
        }

        #endregion
    }
}
