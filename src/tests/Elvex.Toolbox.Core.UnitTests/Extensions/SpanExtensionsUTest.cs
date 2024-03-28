// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Core.UnitTests.Extensions
{
    using NFluent;

    using System;

    /// <summary>
    /// Test for <see cref="SpanExtensions"/>
    /// </summary>
    public sealed class SpanExtensionsUTest
    {
        [Theory]

        // Simple
        [InlineData("azertyuiopqsdfghjklmwxcvbn", '_', (char[])['a', 't', 'p'], "_zer_yuio_qsdfghjklmwxcvbn")]

        // Multiple time same
        [InlineData("azertyuiopqsdfghjklmwxcvbnazertyuiopqsdfghjklmwxcvbn", '_', (char[])['a', 't', 'p'], "_zer_yuio_qsdfghjklmwxcvbn_zer_yuio_qsdfghjklmwxcvbn")]

        // Contigue & last 
        [InlineData("azertyuiopqsdfghjklmwxcvbnazertyuiopqsdfghjklmwxcvbn", '_', (char[])['a', 't', 'p', 'n'], "_zer_yuio_qsdfghjklmwxcvb__zer_yuio_qsdfghjklmwxcvb_")]

        // No replacement
        [InlineData("azertyuiopqsdfghjklmwxcvbnazertyuiopqsdfghjklmwxcvbn", '_', (char[])[], "azertyuiopqsdfghjklmwxcvbnazertyuiopqsdfghjklmwxcvbn")]
        public void Span_ReplaceMany(string source, char replacement, char[] search, string expected)
        {
            ReadOnlySpan<char> spanSource = source;

            var result = spanSource.ReplaceMany(replacement, search);

            Check.That(result.ToString()).IsNotEmpty().And.Equals(expected);
        }
    }
}
