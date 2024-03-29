// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace System
{
    using Elvex.Toolbox.Models;

    using System.Runtime.CompilerServices;

    /// <summary>
    /// Extensions methods about <see cref="string"/>
    /// </summary>
    public static class StringExtensions
    {
        #region Methods

        /// <summary>
        /// Extensions to simplify the call of <see cref="string.Format"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string WithArguments(this string str, params object?[] arguments)
        {
            return string.Format(str, arguments);
        }

        /// <summary>
        /// OptiSplit the string using multiple string as separators with <paramref name="includeSeparator"/> you could keep the separator in the result
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyList<string> OptiSplit(this string str,
                                                      params string[] separators)
        {
            return OptiSplit(str, StringIncludeSeparatorMode.None, StringComparison.Ordinal, StringSplitOptions.None, separators);
        }

        /// <summary>
        /// OptiSplit the string using multiple string as separators with <paramref name="includeSeparator"/> you could keep the separator in the result
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyList<string> OptiSplit(this string str,
                                                      StringComparison comparison,
                                                      StringSplitOptions splitOptions,
                                                      params string[] separators)
        {
            return OptiSplit(str, StringIncludeSeparatorMode.None, comparison, splitOptions, separators);
        }

        /// <summary>
        /// OptiSplit the string using multiple string as separators with <paramref name="includeSeparator"/> you could keep the separator in the result
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyList<string> OptiSplit(this string str,
                                                      StringIncludeSeparatorMode includeSeparator,
                                                      params string[] separators)
        {
            return OptiSplit(str, includeSeparator, StringComparison.Ordinal, StringSplitOptions.None, separators);
        }

        /// <summary>
        /// OptiSplit the string using multiple string as separators with <paramref name="includeSeparator"/> you could keep the separator in the result
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyList<string> OptiSplit(this string str,
                                                      StringComparison comparison,
                                                      params string[] separators)
        {
            return OptiSplit(str, StringIncludeSeparatorMode.None, comparison, StringSplitOptions.None, separators);
        }

        /// <summary>
        /// OptiSplit the string using multiple string as separators with <paramref name="includeSeparator"/> you could keep the separator in the result
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyList<string> OptiSplit(this string str,
                                                      StringSplitOptions splitOptions,
                                                      params string[] separators)
        {
            return OptiSplit(str, StringIncludeSeparatorMode.None, StringComparison.Ordinal, splitOptions, separators);
        }

        /// <summary>
        /// OptiSplit the string using multiple string as separators with <paramref name="includeSeparator"/> you could keep the separator in the result
        /// </summary>
        [Obsolete("Use version with StringIncludeSeparatorMode includeSeparator")]
        public static IReadOnlyList<string> OptiSplit(this string str,
                                                      bool includeSeparator,
                                                      StringComparison comparison,
                                                      StringSplitOptions splitOptions,
                                                      params string[] separators)
        {
            return OptiSplit(str, includeSeparator ? StringIncludeSeparatorMode.Isolated : StringIncludeSeparatorMode.None, comparison, splitOptions, separators);
        }

        /// <summary>
        /// OptiSplit the string using multiple string as separators with <paramref name="includeSeparator"/> you could keep the separator in the result
        /// </summary>
        public static IReadOnlyList<string> OptiSplit(this string str,
                                                      StringIncludeSeparatorMode includeSeparator,
                                                      StringComparison comparison,
                                                      StringSplitOptions splitOptions,
                                                      params string[] separators)

        {
            if (string.IsNullOrEmpty(str))
                return EnumerableHelper<string>.ReadOnlyArray;

            var ordersSeparators = separators.OrderByDescending(s => s.Length).ToArray();
            ReadOnlySpan<char> remainStr = str;
            var index = -1;
            var size = 0;

            var results = new List<string>(str.Length);

            while (true)
            {
                index = -1;
                size = 0;

                foreach (var separator in ordersSeparators)
                {
                    var localIndex = remainStr.IndexOf(separator, comparison);
                    if (localIndex > -1 && (index < 0 || localIndex < index))
                    {
                        index = localIndex;
                        size = separator.Length;
                    }
                }

                if (index < 0 || size == 0)
                    break;

                if (includeSeparator == StringIncludeSeparatorMode.AttachedToPrevious)
                {
                    index = index + size;
                    size = 0;
                }

                if (index > 0)
                    InsertInResult(results, remainStr, 0, index, splitOptions);

                if (includeSeparator == StringIncludeSeparatorMode.Isolated)
                    InsertInResult(results, remainStr, index, size, splitOptions);

                remainStr = remainStr.Slice(index + size);
            }

            if (remainStr.Length > 0)
                results.Add(remainStr.ToString());

            return results;
        }

        /// <summary>
        /// Insert in the collection if allaw by the options
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InsertInResult(in IList<string> results, in ReadOnlySpan<char> remainStr, int index, int size, StringSplitOptions options)
        {
            if (size == 0)
                return;

            string str;

            if ((options & StringSplitOptions.TrimEntries) == StringSplitOptions.TrimEntries)
                str = remainStr.Slice(index, size).Trim().ToString();
            else
                str = remainStr.Slice(index, size).ToString();

            if (str.Length == 0 || ((options & StringSplitOptions.RemoveEmptyEntries) == StringSplitOptions.RemoveEmptyEntries && string.IsNullOrWhiteSpace(str)))
                return;

            results.Add(str);
        }

        #endregion
    }
}
