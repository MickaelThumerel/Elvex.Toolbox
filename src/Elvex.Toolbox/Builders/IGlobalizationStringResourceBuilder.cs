// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Builders
{
    /// <summary>
    /// Builder used to setup Globalization resources
    /// </summary>
    public interface IGlobalizationStringResourceBuilder
    {
        /// <summary>
        /// Add RESXs the string resource.
        /// </summary>
        IGlobalizationStringResourceBuilder RESXStringResource<TResource>();
    }
}
