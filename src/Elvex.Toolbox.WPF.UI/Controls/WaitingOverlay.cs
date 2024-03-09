﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.WPF.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Control" />
    public class WaitingOverlay : Control
    {
        #region Ctor

        /// <summary>
        /// Initializes the <see cref="WaitingOverlay"/> class.
        /// </summary>
        static WaitingOverlay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WaitingOverlay), new FrameworkPropertyMetadata(typeof(WaitingOverlay)));
        }

        #endregion
    }
}
