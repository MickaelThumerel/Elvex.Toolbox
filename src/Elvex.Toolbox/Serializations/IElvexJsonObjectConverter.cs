﻿// Copyright (c) Amexio.

namespace Elvex.Toolbox
{
    using System;

    /// <summary>
    /// Generic converter
    /// </summary>
    public interface IElvexJsonObjectConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified source.
        /// </summary>
        bool CanConvert(Type source);

        /// <summary>
        /// Gets a value indicating whether this instance can write json from the object.
        /// </summary>
        bool CanWrite {  get; }

        /// <summary>
        /// Gets a value indicating whether this instance can read json to produce the dedicated object.
        /// </summary>
        bool CanRead { get; }

        /// <summary>
        /// Reads the json to perform the convertion
        /// </summary>
        bool ReadJson(Func<string, Tuple<bool, object?>> getJsonValue, ref Type objectType, ref object? existingValue);
    }
}
