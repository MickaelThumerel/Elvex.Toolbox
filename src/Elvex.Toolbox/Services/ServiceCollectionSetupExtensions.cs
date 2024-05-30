// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

// KEEP : Microsoft.Extensions.DependencyInjection
namespace Microsoft.Extensions.DependencyInjection
{
    using Elvex.Toolbox.Abstractions.Services;
    using Elvex.Toolbox.Builders;
    using Elvex.Toolbox.Services;

    using Microsoft.Extensions.DependencyInjection.Extensions;

    using System;

    public static class ServiceCollectionSetupExtensions
    {
        /// <summary>
        /// Setups the globalization resources.
        /// </summary>
        public static IServiceCollection SetupGlobalizationResources(this IServiceCollection services, Action<IGlobalizationStringResourceBuilder> resourceBuilder)
        {
            services.TryAddSingleton<IGlobalizationStringResourceProvider, GlobalizationStringResourceProvider>();

            var inst = new GlobalizationStringResourceBuilder(services);
            resourceBuilder(inst);
            return services;
        }
    }
}
