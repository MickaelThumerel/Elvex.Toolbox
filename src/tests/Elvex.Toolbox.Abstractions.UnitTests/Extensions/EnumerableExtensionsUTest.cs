// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.UnitTests.Extensions
{
    using NFluent;

    using System;
    using System.Collections.Generic;

    using Xunit;

    /// <summary>
    /// Test for <see cref="EnumerableExtensions"/>
    /// </summary>
    public sealed class EnumerableExtensionsUTest
    {
        /// <summary>
        /// Enumerables the try get value inline.
        /// </summary>
        [Fact]
        public void Enumerable_TryGetValueInline()
        {
            var dic = new Dictionary<string, int>()
            {
                { "value", 42 }
            };

            // Not Founded without default value
            var defaultNotFounded = dic.TryGetValueInline("NotFounded", out var notFounded);

            Check.That(defaultNotFounded).IsEqualTo(0);
            Check.That(notFounded).IsFalse();

            // Founded
            var foundedValue = dic.TryGetValueInline("value", out var founded, 21);

            Check.That(foundedValue).IsEqualTo(42);
            Check.That(founded).IsTrue();

            // Not Founded without default value
            var defaultNotFoundedWithDefault = dic.TryGetValueInline("NotFounded", out var notFoundedWithDefault, 12);

            Check.That(defaultNotFoundedWithDefault).IsEqualTo(12);
            Check.That(notFoundedWithDefault).IsFalse();
        }
    }
}
