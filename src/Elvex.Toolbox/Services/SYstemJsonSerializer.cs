// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Services
{
    using Elvex.Toolbox.Abstractions.Services;

    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Implementation of <see cref="IJsonSerializer"/> used .net building serializer
    /// </summary>
    /// <seealso cref="IJsonSerializer" />
    public sealed class SystemJsonSerializer : IJsonSerializer
    {
        #region Fields

        private static readonly JsonSerializerOptions s_defaultDeserializationOptions;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes the <see cref="SystemJsonSerializer"/> class.
        /// </summary>
        static SystemJsonSerializer()
        {
            s_defaultDeserializationOptions = new JsonSerializerOptions()
            {
                IncludeFields = true,
                PropertyNameCaseInsensitive = false,
                WriteIndented = Debugger.IsAttached,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
            };

            Instance = new SystemJsonSerializer();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static IJsonSerializer Instance { get; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public object? Deserialize(string json, Type returnType)
        {
            return JsonSerializer.Deserialize(json, returnType, s_defaultDeserializationOptions);
        }

        /// <inheritdoc />
        public object? Deserialize(Stream stream, Type returnType)
        {
            return JsonSerializer.Deserialize(stream, returnType, s_defaultDeserializationOptions);
        }

        /// <inheritdoc />
        public TResult? Deserialize<TResult>(Stream stream)
        {
            return JsonSerializer.Deserialize<TResult>(stream, s_defaultDeserializationOptions);
        }

        /// <inheritdoc />
        public TResult? Deserialize<TResult>(string json)
        {
            return JsonSerializer.Deserialize<TResult>(json, s_defaultDeserializationOptions);
        }

        /// <inheritdoc />
        public object? Deserialize(in ReadOnlySpan<byte> str, Type returnType)
        {
            return JsonSerializer.Deserialize(str, returnType, s_defaultDeserializationOptions);
        }

        /// <inheritdoc />
        public TResult? Deserialize<TResult>(in ReadOnlySpan<byte> str)
        {
            return JsonSerializer.Deserialize<TResult>(str, s_defaultDeserializationOptions);
        }

        /// <inheritdoc />
        public byte[] Serialize<TObject>(TObject obj)
        {
            var str = JsonSerializer.Serialize<TObject>(obj, s_defaultDeserializationOptions);
            return Encoding.UTF8.GetBytes(str);
        }

        #endregion
    }
}
