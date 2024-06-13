// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]    
    public sealed class RefRouteAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefRouteAttribute"/> class.
        /// </summary>
        public RefRouteAttribute(string route)
        {
            this.Route = route;                
        }

        /// <summary>
        /// Gets the route.
        /// </summary>
        public string Route { get; }
    }
}
