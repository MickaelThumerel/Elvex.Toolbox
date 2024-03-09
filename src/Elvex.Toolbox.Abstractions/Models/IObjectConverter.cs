﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Models
{
    using System;

    /// <summary>
    /// Converter object using only <see cref="IDedicatedObjectConverter"/>
    /// </summary>
    public interface IObjectConverter
    {
        /// <summary>
        /// Converts <paramref name="source"/> to specific typ <typeparamref name="TConvertedObject"/>
        /// </summary>
        bool TryConvert<TConvertedObject>(in object? source, out TConvertedObject? result);

        /// <summary>
        /// Converts <paramref name="source"/> to specific typ <typeparamref name="TConvertedObject"/>
        /// </summary>
        bool TryConvert(in object? source, Type targetType, out object? result);
    }
}
