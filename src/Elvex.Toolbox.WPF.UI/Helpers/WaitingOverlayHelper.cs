﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.WPF.UI.Helpers
{
    using Elvex.Toolbox.WPF.UI.Controls;

    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    /// <summary>
    /// 
    /// </summary>
    public sealed class WaitingOverlayHelper
    {
        #region Fields

        public static readonly DependencyProperty EnabledProperty = DependencyProperty.RegisterAttached(nameof(GetEnabled).Substring(3),
                                                                                                        typeof(bool),
                                                                                                        typeof(WaitingOverlayHelper),
                                                                                                        new FrameworkPropertyMetadata(false,
                                                                                                                                      propertyChangedCallback: OnOverlayEnabledChanged));

        #endregion

        #region Properties

        /// <summary>
        /// Gets the enabled.
        /// </summary>
        public static bool GetEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnabledProperty);
        }

        /// <summary>
        /// Sets the enabled.
        /// </summary>
        public static void SetEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(EnabledProperty, value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when [overlay enabled changed].
        /// </summary>
        private static void OnOverlayEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var inst = d as ContentControl;

            if (inst == null)
                return;

            if (e.NewValue is bool bValue)
            {
                if (bValue)
                {
                    if (inst.IsLoaded == false)
                    {
                        inst.Loaded -= EnabledAdorner;
                        inst.Loaded += EnabledAdorner;
                        return;
                    }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    EnabledAdorner(inst, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                }
            }
        }

        private static void EnabledAdorner(object sender, RoutedEventArgs e)
        {
            var inst = sender as ContentControl;

            if (inst is null)
                return;

            var content = inst.Content as UIElement;
            AdornerLayer? adornerLayer = null;

            if (content is AdornerDecorator decorator)
            {
                content = decorator.Child as UIElement;
                adornerLayer = decorator.AdornerLayer;
            }

            if (adornerLayer is null)
            {
                var adornerDecorator = new AdornerDecorator();
                inst.Content = adornerDecorator;
                adornerDecorator.Child = content;
                adornerLayer = adornerDecorator.AdornerLayer;
            }

            if (content is null)
                return;

            var adorner = new AdornerContentPresenter<WaitingOverlay>(content);
            adornerLayer.Add(adorner);
        }

        #endregion
    }
}
