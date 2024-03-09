// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Statements
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public interface IBoolLogicStatement
    {
        /// <summary>
        /// Ask is statement is valid
        /// </summary>
        /// <param name="input">The inputs variables.</param>
        bool Ask(in ReadOnlySpan<bool> input);
    }
}
