﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Memories
{
    /// <summary>
    /// Define a memory range
    /// </summary>
    public struct MemoryRange
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryRange"/> struct.
        /// </summary>
        public MemoryRange(int start, int length)
        {
            this.Start = start;
            this.Length = length;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the length.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Gets the start.
        /// </summary>
        public int Start { get; set; }

        #endregion
    }
}
