﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Helpers
{
    using System;
    using System.Linq;
    using System.Net.NetworkInformation;

    public static class NetworkHelper
    {
        /// <summary>
        /// Gets the next unused port.
        /// </summary>
        /// <exception cref="System.ArgumentException">Max cannot be less than min.</exception>
        /// <exception cref="System.Exception">All local TCP ports between {min} and {max} are currently in use.</exception>
        /// <remarks>
        ///     https://stackoverflow.com/questions/138043/find-the-next-tcp-port-in-net
        /// </remarks>
        public static int GetNextUnusedPort(int min, int max, params int[] exceptionList)
        {
            if (max < min)
                throw new ArgumentException("Max cannot be less than min.");

            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();

            var usedPorts = ipProperties.GetActiveTcpConnections()
                                        .Where(connection => connection.State != TcpState.Closed)
                                        .Select(connection => connection.LocalEndPoint)
                                        .Concat(ipProperties.GetActiveTcpListeners())
                                        .Concat(ipProperties.GetActiveUdpListeners())
                                        .Select(endpoint => endpoint.Port)
                                        .Concat(exceptionList)
                                        .ToArray();

            var firstUnused = Enumerable.Range(min, max - min)
                                        .Where(port => !usedPorts.Contains(port))
                                        .Select(port => new int?(port))
                                        .FirstOrDefault();

            if (!firstUnused.HasValue)
                throw new Exception($"All local TCP ports between {min} and {max} are currently in use.");

            return firstUnused.Value;
        }
    }
}
