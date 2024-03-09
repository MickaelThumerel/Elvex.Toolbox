// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Proxies
{
    using Elvex.Toolbox.Abstractions.Enums;

    /// <summary>
    /// Define a service used store and distribute <see cref="IDispatcherProxy"/> as needed
    /// </summary>
    public interface IDispatcherProxyStore
    {
        /// <summary>
        /// Gets the <see cref="IDispatcherProxy"/> associate to <see cref="DispatcherTypeEnum"/>; <br/> 
        /// <c>null</c> if no <see cref="IDispatcherProxy"/> have been registred
        /// </summary>
        IDispatcherProxy? GetDispatcherProxy(DispatcherTypeEnum dispatcherType);

        /// <summary>
        /// Gets the <see cref="IDispatcherProxy"/> associate to <see cref="DispatcherTypeEnum"/>; <br/> 
        /// <c>null</c> if no <see cref="IDispatcherProxy"/> have been registred
        /// </summary>
        /// <exception cref="InvalidOperationException">No dispatcher are in the store for type  <paramref name="dispatcherType"/></exception>
        IDispatcherProxy GetRequiredDispatcherProxy(DispatcherTypeEnum dispatcherType);
    }
}