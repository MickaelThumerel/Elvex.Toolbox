﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Attributes
{
    using System.ComponentModel;

    /// <summary>
    /// Description with a char alias
    /// </summary>
    /// <seealso cref="System.ComponentModel.DescriptionAttribute" />
    public sealed class DescriptionWithAliasAttribute : DescriptionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionWithAliasAttribute"/> class.
        /// </summary>
        public DescriptionWithAliasAttribute(char alias, string? description = null)
            : base(description ?? string.Empty)
        {
            this.Alias = alias;
        }

        /// <summary>
        /// Gets the alias.
        /// </summary>
        public char Alias { get; }
    }
}
