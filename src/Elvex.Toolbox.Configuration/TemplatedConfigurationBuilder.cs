﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Configuration
{
    using Microsoft.Extensions.Configuration;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class TemplatedConfigurationBuilder : IConfigurationBuilder
    {
        #region Fields

        private readonly IConfigurationBuilder _rootBuilder;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplatedConfigurationBuilder"/> class.
        /// </summary>
        internal TemplatedConfigurationBuilder(IConfigurationBuilder rootbuilder)
        {
            this._rootBuilder = rootbuilder;
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public IDictionary<string, object> Properties
        {
            get { return this._rootBuilder.Properties; }
        }

        /// <inheritdoc />
        public IList<IConfigurationSource> Sources
        {
            get { return this._rootBuilder.Sources; }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public IConfigurationBuilder Add(IConfigurationSource source)
        {
            ArgumentNullException.ThrowIfNull(source);
            return this._rootBuilder.Add(source);
        }

        /// <inheritdoc />
        public IConfigurationRoot Build()
        {
            var providers = this._rootBuilder.Sources
                                             .Select(s => s.Build(this))
                                             .Select(provider => new TemplatedConfigurationProxySourceProvider(provider))
                                             .ToArray();

            return new ConfigurationRoot(providers);
        }

        #endregion
    }
}
