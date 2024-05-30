﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.WPF.UI.Services
{
    using Elvex.Toolbox.Abstractions.Proxies;

    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Threading;

    /// <summary>
    /// Dispatcher on UI dispatcher
    /// </summary>
    /// <seealso cref="IDispatcherProxy" />
    public sealed class UIDispatcher : IDispatcherProxy
    {
        #region Fields

        private readonly Dispatcher _dispatcher;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="UIDispatcher"/> class.
        /// </summary>
        public UIDispatcher(Dispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance run in the dispatcher thread.
        /// </summary>
        public bool IsCurrentThread
        {
            get { return this._dispatcher.Thread == Thread.CurrentThread; }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Send(Action callback)
        {
            if (this.IsCurrentThread)
                callback();
            else
                this._dispatcher.Invoke(callback);
        }

        /// <inheritdoc />
        public void Throw(Exception ex, [CallerMemberName] string? callerMemberName = null)
        {
            Send(() => throw new Exception("Exception raised by " + callerMemberName + "\n StackTrace : " + ex.StackTrace, ex));
        }

        #endregion
    }
}
