﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.WPF.UI.Controls
{
    using Elvex.Toolbox.WPF.Commands;

    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Window extension
    /// </summary>
    /// <seealso cref="Window" />
    public class WindowExt : Window
    {
        #region Fields

        private static readonly DependencyPropertyKey s_closeCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CloseCommand),
                                                                                                                      typeof(ICommand),
                                                                                                                      typeof(WindowExt),
                                                                                                                      new PropertyMetadata(null));

        public static readonly DependencyProperty CloseCommandProperty = s_closeCommandPropertyKey.DependencyProperty;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowExt"/> class.
        /// </summary>
        public WindowExt()
        {
            SetValue(s_closeCommandPropertyKey, new DelegateCommand(() => Close()));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the close command.
        /// </summary>
        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:Closed" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnClosed(EventArgs e)
        {
            if (this.DataContext is IDisposable disposable)
                disposable.Dispose();

            base.OnClosed(e);
        }

        #endregion
    }
}
