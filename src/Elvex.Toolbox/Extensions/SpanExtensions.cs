// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace System
{
    using System.Buffers;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// 
    /// </summary>
    public static class SpanExtensions
    {
        /// <summary>
        /// Counts number of time the value <paramref name="valueToCount"/> is in <paramref name="values"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Count<T>(in this ReadOnlySpan<T> values, T valueToCount)
        {
            return Count(values, (item) => EqualityComparer<T>.Default.Equals(item, valueToCount));
        }

        /// <summary>
        /// Counts number of time the <paramref name="valueToCountPredicate"/> is valid
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Count<T>(in this ReadOnlySpan<T> values, Func<T?, bool> valueToCountPredicate)
        {
            uint counter = 0;
            foreach (var item in values)
            {
                if (valueToCountPredicate(item))
                    counter++;
            }

            return counter;
        }

        /// <summary>
        /// Replaces many char by a new value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<char> ReplaceMany(in this ReadOnlySpan<char> source, in char replacement, in ReadOnlySpan<char> values)
        {
            var result = new Span<char>(source.ToArray());

            result.ReplaceMany(replacement, values);

            return result;
        }

        /// <summary>
        /// Replaces many char by a new value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<char> ReplaceMany(in this Span<char> source, in char replacement, in ReadOnlySpan<char> values)
        {
            var searchValues = SearchValues.Create(values);

            var workSpan = source;

            var searchIndx = workSpan.IndexOfAny(searchValues);
            while (searchIndx > -1)
            {
                workSpan[searchIndx] = replacement;

                if (searchIndx + 1 >= source.Length)
                    break;

                workSpan = workSpan.Slice(searchIndx + 1);
                searchIndx = workSpan.IndexOfAny(searchValues);
            }

            return source;
        }
    }
}
