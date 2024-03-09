// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Enums
{
    using Elvex.Toolbox.Abstractions.Attributes;

    /// <summary>
    /// Enum expositing logical relation
    /// </summary>
    public enum LogicEnum
    {
        None,

        [DescriptionWithAlias('&')]
        And,

        [DescriptionWithAlias('|')]
        Or,

        [DescriptionWithAlias('^')]
        ExclusiveOr,

        [DescriptionWithAlias('!')]
        Not
    }
}
