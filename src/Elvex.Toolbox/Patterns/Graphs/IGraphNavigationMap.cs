﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Patterns.Graphs
{
    using Elvex.Toolbox.Patterns.Graphs.Map;

    using System.Collections.Generic;

    /// <summary>
    /// Map used to navigate through an <see cref="IGraph"/>
    /// </summary>
    public interface IGraphNavigationMap
    {
        /// <summary>
        /// Gets the shortest path from <paramref name="source"/> to <paramref name="target"/>, using specific relation types.
        /// </summary>
        /// <remarks>
        ///     If not <paramref name="relationTypes"/> are specify all the relation are used
        /// </remarks>
        IReadOnlyCollection<IGraphNode> GetShortestPath(IGraphNode source, IGraphNode target, GetPathOptions? options = null);
    }
}