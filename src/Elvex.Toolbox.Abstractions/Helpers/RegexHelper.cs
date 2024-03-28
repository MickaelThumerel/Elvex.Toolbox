// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Helpers
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// Helper Around regex
    /// </summary>
    public static class RegexHelper
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        static RegexHelper()
        {
            MultiSpace = new Regex(Pattern.MULTI_SPACE, RegexOptions.Compiled);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the multi space.
        /// </summary>
        public static Regex MultiSpace { get; }

        #endregion

        #region Nested

        /// <summary>
        /// Define regex classic pattern
        /// </summary>
        public static class Pattern
        {
            /// <summary>
            /// The unique identifier pattern
            /// </summary>
            public const string GUID = "[a-zA-Z0-9]{8}[-]?[a-zA-Z0-9]{4}[-]?[a-zA-Z0-9]{4}[-]?[a-zA-Z0-9]{4}[-]?[a-zA-Z0-9]{12}";

            /// <summary>
            /// The multi space
            /// </summary>
            public const string MULTI_SPACE = "[\\s]+";
        }

        #endregion
    }
}
