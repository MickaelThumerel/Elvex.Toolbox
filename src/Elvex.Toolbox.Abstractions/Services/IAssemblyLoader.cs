﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Services
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Define a service in charge to load an assembly from different source
    /// </summary>
    public interface IAssemblyLoader
    {
        /// <summary>
        /// Loads the assembly located <paramref name="path"/> in the current <see cref="AppDomain"/>.
        /// </summary>
        Assembly Load(string path);

        /// <summary>
        /// Loads the assembly from <paramref name="stream"/> in the current <see cref="AppDomain"/>.
        /// </summary>
        Assembly Load(Stream stream);

        /// <summary>
        /// Loads the assembly from <paramref name="assembly"/> in the current <see cref="AppDomain"/>.
        /// </summary>
        Assembly Load(AssemblyName assembly);
    }
}
