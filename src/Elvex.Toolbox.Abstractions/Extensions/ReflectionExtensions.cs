﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace System.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Extensions methods about reflection tools
    /// </summary>
    public static class ReflectionExtensions
    {
        #region Fields

        private static readonly Dictionary<MethodBase, string> s_methodUniqueId;
        private static readonly HashSet<string> s_methodUniqueIdHashset;

        private static readonly ReaderWriterLockSlim s_methodUniqueIdLocker;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes the <see cref="ReflectionExtensions"/> class.
        /// </summary>
        static ReflectionExtensions()
        {
            s_methodUniqueIdLocker = new ReaderWriterLockSlim();
            s_methodUniqueId = new Dictionary<MethodBase, string>();
            s_methodUniqueIdHashset = new HashSet<string>();
        }

        #endregion

        #region Method

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <remarks>
        ///     This unique ID is only guarenty unique by <see cref="AppDomain"/>
        ///     We use TypeFullName + MethodFUllName hash by MD5.
        ///     We use cache to guaranty it's unique if the Hash produce is the same a Guid is introduce to add hazard
        /// </remarks>
        public static string GetUniqueId(this MethodBase methodInfo, bool raisedErrorIfDuplicate = false, bool useCache = true)
        {
            if (useCache)
            {
                s_methodUniqueIdLocker.EnterReadLock();

                try
                {
                    if (s_methodUniqueId.TryGetValue(methodInfo, out var uniqueKey))
                        return uniqueKey;
                }
                finally
                {
                    s_methodUniqueIdLocker.ExitReadLock();
                }
            }

            s_methodUniqueIdLocker.EnterWriteLock();

            try
            {
                if (useCache && s_methodUniqueId.TryGetValue(methodInfo, out var uniqueKey))
                    return uniqueKey;

                string hashKey = string.Empty;
                bool cacheContainsHash = false;
                Guid? randomKeyPart = null;

                do
                {
                    if (cacheContainsHash)
                        randomKeyPart = Guid.NewGuid();

                    hashKey = BuildMethodUniqueId(methodInfo, randomKeyPart);

                    if (useCache == false)
                        break;

                    cacheContainsHash = s_methodUniqueIdHashset.Contains(hashKey);

                    if (raisedErrorIfDuplicate && cacheContainsHash)
                        throw new InvalidOperationException("Could not genenerate unique id for " + methodInfo + " because another method have the same hash =>" + s_methodUniqueId.First(m => m.Value == hashKey));
                }
                while (string.IsNullOrEmpty(hashKey) || cacheContainsHash);

                if (useCache)
                {
                    s_methodUniqueId.Add(methodInfo, hashKey);
                    s_methodUniqueIdHashset.Add(hashKey);
                }

                return hashKey;
            }
            finally
            {
                s_methodUniqueIdLocker.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public static string GetDisplayName(this MethodBase methodInfo)
        {
            var keyBuilder = new StringBuilder();

            keyBuilder.Append("[");
            keyBuilder.Append(methodInfo.DeclaringType?.GetTypeInfoExtension().FullShortName);
            keyBuilder.Append("] ");

            BuildMethodDisplayName(methodInfo, keyBuilder);

            return keyBuilder.ToString();
        }

        #region Tools

        /// <summary>
        /// Builds a display name for method <paramref name="methodInfo"/>
        /// </summary>
        private static void BuildMethodDisplayName(this MethodBase methodInfo, StringBuilder keyBuilder)
        {
            if (methodInfo is MethodInfo mInfo)
            {
                keyBuilder.Append(mInfo.ReturnType.FullName);
                keyBuilder.Append(" ");
            }

            keyBuilder.Append(methodInfo.Name);

            if (methodInfo.IsGenericMethod)
            {
                keyBuilder.Append("<");

                bool first = true;
                foreach (var geneParam in methodInfo.GetGenericArguments())
                {
                    if (!first)
                        keyBuilder.Append(",");

                    first = false;
                    keyBuilder.Append(geneParam.GetAbstractType().DisplayName);
                }

                keyBuilder.Append(">");
            }

            keyBuilder.Append("(");

            bool firstParams = true;
            foreach (var param in methodInfo.GetParameters())
            {
                if (!firstParams)
                    keyBuilder.Append(",");

                firstParams = false;
                keyBuilder.Append(param.ParameterType.GetAbstractType().DisplayName);
            }

            keyBuilder.Append(")");
        }

        /// <summary>
        /// Builds the method unique identifier.
        /// </summary>
        private static string BuildMethodUniqueId(MethodBase methodInfo, Guid? randomFactor)
        {
            var keyBuilder = new StringBuilder();

            BuildMethodDisplayName(methodInfo, keyBuilder);

            if (randomFactor.HasValue)
            {
                keyBuilder.Append(':');
                keyBuilder.Append(randomFactor.Value);
            }

            using (var hash = SHA512.Create())
            {
                var keyBytes = Encoding.UTF8.GetBytes(keyBuilder.ToString());
                var hashKeyBytes = hash.ComputeHash(keyBytes);

                keyBuilder.Clear();

                var shortName = methodInfo.DeclaringType?.GetTypeInfoExtension().FullShortName ?? string.Empty;
                keyBuilder.EnsureCapacity((keyBytes.Length * 2) + shortName.Length + 1);

                keyBuilder.Append(shortName);
                keyBuilder.Append("_");
                keyBuilder.Append(Convert.ToBase64String(hashKeyBytes));

                return keyBuilder.ToString();
            }
        }

        #endregion

        #endregion
    }
}
