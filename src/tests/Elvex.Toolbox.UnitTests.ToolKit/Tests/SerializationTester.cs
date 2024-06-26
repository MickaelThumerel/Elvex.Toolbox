﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.UnitTests.ToolKit.Tests
{
    using Newtonsoft.Json;

    using NFluent;

    using System;

    public static class SerializationTester
    {
        /// <summary>
        /// Test item serialization 
        /// </summary>
        public static void SerializeTester<TItem>(TItem item)
            where TItem : class, IEquatable<TItem>
        {
            var serializeSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
            };

            var json = JsonConvert.SerializeObject(item, serializeSettings);

            Check.That(json).IsNotNull().And.IsNotEmpty();

            var deserializedDef = JsonConvert.DeserializeObject(json, serializeSettings);
            Check.That(deserializedDef).IsNotNull()
                                       .And
                                       .IsInstanceOfType(item.GetType())
                                       .And
                                       .Not.IsSameReferenceAs(item)
                                       .And
                                       .IsEqualTo(item);

            var rejson = JsonConvert.SerializeObject(deserializedDef, serializeSettings);

            Check.That(rejson).IsEqualTo(json);
        }
    }
}
