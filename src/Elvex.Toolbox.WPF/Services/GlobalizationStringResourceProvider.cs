// Copyright (c) Nexai.
// This file is licenses to you under the MIT license.
// Produce by nexai, elvexoft & community (cf. docs/Teams.md)

namespace Elvex.Toolbox.WPF.Services
{
    using Elvex.Toolbox.Disposables;
    using Elvex.Toolbox.Patterns.Strategy;
    using Elvex.Toolbox.WPF.Abstractions.Services;

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// String Resource provider throught appicaltion
    /// </summary>
    /// <seealso cref="ProviderStrategyBase{string, string, IStringResourceSourceProvider}" />
    /// <seealso cref="IGlobalizationStringResourceProvider" />
    internal sealed class GlobalizationStringResourceProvider : SafeDisposable, IGlobalizationStringResourceProvider
    {
        #region Fields

        private readonly IReadOnlyCollection<IGlobalizationStringResourceSourceProvider> _sourceProviders;

        private readonly Dictionary<string, Dictionary<string, string?>> _stringCache;
        private readonly ReaderWriterLockSlim _cacheLocker;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalizationStringResourceProvider"/> class.
        /// </summary>
        public GlobalizationStringResourceProvider(IEnumerable<IGlobalizationStringResourceSourceProvider> sourceProviders)
        {
            this._sourceProviders = sourceProviders?.ToArray() ?? EnumerableHelper<IGlobalizationStringResourceSourceProvider>.ReadOnlyArray;

            this._stringCache = new Dictionary<string, Dictionary<string, string?>>();
            this._cacheLocker = new ReaderWriterLockSlim();
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public string GetResource(string name, CultureInfo? forceCultureInfo = null)
        {
            if (string.IsNullOrEmpty(name))
                return name;

            var culture = forceCultureInfo ?? CultureInfo.CurrentCulture;
            var cultureKey = culture.ThreeLetterISOLanguageName;

            this._cacheLocker.EnterReadLock();
            try
            {
                if (this._stringCache.TryGetValue(cultureKey, out var cacheByLanguage) &&
                    cacheByLanguage.TryGetValue(name, out var cachedResult))
                {
                    return cachedResult ?? name;
                }
            }
            finally
            {
                this._cacheLocker.ExitReadLock();
            }

            bool founded = false;
            string? result = name;

            foreach (var provider in this._sourceProviders)
            {
                if (provider.TryGetResource(name, culture, out var resultFromSource))
                {
                    result = resultFromSource;
                    founded = true;
                    break;
                }
            }

            if (founded)
            {
                this._cacheLocker.EnterWriteLock();
                try
                {
                    Dictionary<string, string?>? newCacheByLanguage = null;
                    if (!this._stringCache.TryGetValue(cultureKey, out newCacheByLanguage))
                    {
                        newCacheByLanguage = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
                        this._stringCache.Add(cultureKey, newCacheByLanguage);
                    }

                    if (!newCacheByLanguage.ContainsKey(name))
                    {
                        newCacheByLanguage.Add(name, result);
                    }
                }
                finally
                {
                    this._cacheLocker.ExitWriteLock();
                }
            }

            return result ?? name;
        }

        #endregion
    }
}
