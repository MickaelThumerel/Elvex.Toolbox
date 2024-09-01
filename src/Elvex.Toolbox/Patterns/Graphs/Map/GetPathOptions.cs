// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Patterns.Graphs.Map
{
    using System.Collections.Generic;

    /// <summary>
    /// Define option to build the node path
    /// </summary>
    public record struct GetPathOptions(int MaxToleratePathSize = -1,
                                        IReadOnlyCollection<string>? FilterRelationTypes = null,
                                        IReadOnlyCollection<string>? ExcludeRelationTypes = null);
}