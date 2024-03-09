// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.UnitTests.Extensions
{
    using NFluent;

    using System;

    /// <summary>
    /// Test for <see cref="StringExtensions"/>
    /// </summary>
    public sealed class StringExtensionsUTest
    {
        [Theory]
        [InlineData("List", false, StringSplitOptions.RemoveEmptyEntries, new[] { "<<", ">>" }, "List")]
        [InlineData("List<<int>>", false, StringSplitOptions.RemoveEmptyEntries, new[] { "<<", ">>" }, "List", "int")]
        [InlineData("List<<int>>", true, StringSplitOptions.RemoveEmptyEntries, new[] { "<<", ">>" }, "List", "<<", "int", ">>")]
        [InlineData("<<List<<int>>", true, StringSplitOptions.RemoveEmptyEntries, new[] { "<<", ">>" }, "<<", "List", "<<", "int", ">>")]
        [InlineData("    List`3<<int, double, List<<string, double, float>>   >>",
                    true,
                    StringSplitOptions.RemoveEmptyEntries,
                    new[] { "<<", ">>", ",", " " },
                    "List`3", "<<", "int", ",", "double", ",", "List", "<<", "string", ",", "double", ",", "float", ">>", ">>")]

        [InlineData("    List`3<<int, double, List<< <string, double, float>>   >>",
                    true,
                    StringSplitOptions.TrimEntries,
                    new[] { "<<", ">>", ",", " ", "<" },
                    "List`3", "<<", "int", ",", "double", ",", "List", "<<", "<", "string", ",", "double", ",", "float", ">>", ">>")]

        [InlineData("    List`3<<int, double, List<< <string, double, float>>   >>",
                    false,
                    StringSplitOptions.TrimEntries,
                    new[] { "<<", ">>", ",", " ", "<" },
                    "List`3", "int", "double", "List", "string", "double", "float")]
        public void SplitByMultipleSeparators(string source, bool includeSeparator, StringSplitOptions options, string[] separators, params string[] expectedResults)
        {
            var args = source.OptiSplit(includeSeparator, StringComparison.Ordinal, options, separators);

            Check.That(args).IsNotNull()
                            .And
                            .CountIs(expectedResults.Length)
                            .And
                            .ContainsExactly(expectedResults);
        }
    }
}
