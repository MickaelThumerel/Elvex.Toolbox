// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Patterns.Graphs.Map
{
    using System.Collections.Generic;

    /// <summary>
    /// Serializable information for the <see cref="DijkstraGraphNavigationMap"/>
    /// </summary>
    public sealed record class DijkstraGraphNavigationMapDefinition(IReadOnlyCollection<string> Nodes, int[][]? Map);
}