﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Conditions
{
    using Elvex.Toolbox.Models;

    using Newtonsoft.Json;

    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>
    /// Store simple value
    /// </summary>
    [DataContract]
    [Serializable]
    [ImmutableObject(true)]
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    public sealed class ConditionValueDefinition : ConditionBaseDefinition
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    {
        #region Fields

        public const string TypeDiscriminator = "value";

        private readonly IEqualityComparer _comparer;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionValueDefinition"/> class.
        /// </summary>
        public ConditionValueDefinition(AbstractType type, object? value)
        {
            this.Type = type;
            this.Value = value;

            if (value is not null && type.IsEqualTo(value.GetType()) == false)
            {
                var serialized = JsonConvert.SerializeObject(value);
                this.Value = JsonConvert.DeserializeObject(serialized, type.ToType());
            }

            this._comparer = BuildComparer(type.ToType());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type.
        /// </summary>
        [DataMember]
        public AbstractType Type { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        [Newtonsoft.Json.JsonProperty(TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All)]
        [DataMember]
        public object? Value { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        public static bool operator !=(ConditionValueDefinition x, ConditionValueDefinition y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        public static bool operator ==(ConditionValueDefinition x, ConditionValueDefinition y)
        {
            return x?.Equals(y) ?? y is null;
        }

        /// <inheritdoc />
        protected override bool OnEquals(ConditionBaseDefinition other)
        {
            return other is ConditionValueDefinition val &&
                   this.Type.Equals(val.Type) &&
                   this._comparer.Equals(this.Value, val.Value);
        }

        /// <inheritdoc />
        protected override int OnGetHashCode()
        {
            return HashCode.Combine(this.Type, this.Value);
        }

        /// <summary>
        /// Builds the comparer.
        /// </summary>
        private static IEqualityComparer BuildComparer(Type type)
        {
            // OPTIMIZE : cache comparer by type
            return (IEqualityComparer)(typeof(EqualityComparer<>).MakeGenericType(type)
                                                                 .GetProperty(nameof(EqualityComparer<int>.Default), BindingFlags.Public | BindingFlags.Static)!
                                                                 .GetValue(null))!;
        }

        #endregion
    }
}
