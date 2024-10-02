﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Extensions
{
    using Elvex.Toolbox.Abstractions.Supports;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{ToDebugDisplayName()}")]
    public record struct NavigationPath<TSourceNode, TPathPart>(IReadOnlyCollection<TPathPart> Parts) : ISupportDebugDisplayName
    {
        #region Ctor

        /// <summary>
        /// .cctors this instance.
        /// </summary>
        static NavigationPath()
        {
            Empty = new NavigationPath<TSourceNode, TPathPart>(EnumerableHelper<TPathPart>.ReadOnly);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the empty.
        /// </summary>
        public static NavigationPath<TSourceNode, TPathPart> Empty { get; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public string ToDebugDisplayName()
        {
            return string.Join("/", this.Parts.Select(p => p is ISupportDebugDisplayName s ? s.ToDebugDisplayName() : p?.ToString()));
        }

        /// <summary>
        /// Founds the node by path.
        /// </summary>
        public TSourceNode FoundNodeByPath(TSourceNode source,
                                           Func<TSourceNode, IEnumerable<TSourceNode>> getChildren,
                                           Func<TSourceNode, ushort, TSourceNode, TPathPart, bool> equality)
        {
            var current = source;
            foreach (var part in this.Parts.Skip(1))
            {
                var children = getChildren(current);
                ushort index = 0;

                foreach (var c in children)
                {
                    if (equality(current, index, c, part))
                    {
                        current = c;
                        break;
                    }
                    index++;
                }
            }

            return current;
        }

        #endregion
    }
}
