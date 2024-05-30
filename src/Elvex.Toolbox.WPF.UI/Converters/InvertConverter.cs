﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.WPF.UI.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(bool), typeof(bool))]
    public sealed class InvertConverter : IValueConverter, ISupportGroupConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool bValue)
                return !bValue;

            return Binding.DoNothing;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool bValue)
                return !bValue;

            return Binding.DoNothing;
        }
    }
}
