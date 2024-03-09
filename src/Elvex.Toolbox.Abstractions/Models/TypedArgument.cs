﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Models
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>
    /// Used to store argument list in mode LinkList to have a strong type container
    /// </summary>
    [DataContract]
    [Serializable]
    [ImmutableObject(true)]

    [KnownType(typeof(TypedArgument<>))]

    public abstract class TypedArgument
    {
        #region Fields

        private static readonly MethodInfo s_fromGenericMethod;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes the <see cref="TypedArgument"/> class.
        /// </summary>
        static TypedArgument()
        {
            s_fromGenericMethod = typeof(TypedArgument).GetMethod(nameof(FromGeneric), BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidDataException("Method FromGeneric not founded");

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedArgument"/> class.
        /// </summary>
        protected TypedArgument(TypedArgument? next)
        {
            this.Next = next;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the next argument
        /// </summary>
        [DataMember]
        public TypedArgument? Next { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Flatterns argument list
        /// </summary>
        public IReadOnlyCollection<object?> Flattern()
        {
            return FlatternImpl().ToReadOnly();
        }

        /// <summary>
        /// Flatterns argument list
        /// </summary>
        private IEnumerable<object?> FlatternImpl()
        {
            yield return GetValue();

            if (this.Next != null)
            {
                foreach (var childValue in this.Next.Flattern())
                {
                    yield return childValue;
                }
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public abstract object? GetValue();

        /// <summary>
        /// Construction argument link list
        /// </summary>
        public static TypedArgument? From(params Type[] types)
        {
            return TypedArgument.From(types.Select(t => t.GetTypeInfoExtension().Default), types, null, 0);
        }

        /// <summary>
        /// Construction argument link list
        /// </summary>
        public static TypedArgument? From(object? objects,
                                          Type providedTypes,
                                          Func<object?, Type, int, bool>? nullIfConditionFalse = null,
                                          int depth = 0)
        {
            return TypedArgument.From(new[] { objects }, new[] { providedTypes }, nullIfConditionFalse, depth);
        }

        /// <summary>
        /// Construction argument link list
        /// </summary>
        public static TypedArgument? From(IEnumerable<object?> objects,
                                          IEnumerable<Type> providedTypes,
                                          Func<object?, Type, int, bool>? nullIfConditionFalse = null,
                                          int depth = 0)
        {
            if (objects == null || !objects.Any())
                return null;

            var current = objects.FirstOrDefault();
            var currentType = current?.GetType() ?? providedTypes?.FirstOrDefault() ?? typeof(object);

            providedTypes ??= EnumerableHelper<Type>.ReadOnly;

            var tail = objects.Skip(1);

            if (nullIfConditionFalse != null && nullIfConditionFalse(current, currentType, depth) == false)
            {
                current = null;
            }

            var tailProvidedTypes = providedTypes.Skip(1);

            var args = new object?[]
            {
                current,
                tail,
                tailProvidedTypes,
                nullIfConditionFalse,
                depth
            };

            var arg = s_fromGenericMethod.MakeGenericMethod(currentType).Invoke(null, args);
            Debug.Assert(arg != null);

            return (TypedArgument)arg;
        }

        /// <summary>
        /// Create specific container
        /// </summary>
        private static TypedArgument<TType> FromGeneric<TType>(TType arg,
                                                               IEnumerable<object> tail,
                                                               IEnumerable<Type> providedTypes,
                                                               Func<object?, Type, int, bool>? nullIfConditionFalse = null,
                                                               int depth = 0)
        {
            return new TypedArgument<TType>(arg, From(tail, providedTypes, nullIfConditionFalse, depth + 1));
        }

        #endregion
    }

    /// <summary>
    /// Used to store argument list in mode LinkList to have a strong type container
    /// </summary>
    [DataContract]
    [Serializable]
    [ImmutableObject(true)]
    public sealed class TypedArgument<TArg> : TypedArgument
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedArgument{TArg}"/> class.
        /// </summary>
        public TypedArgument(TArg value, TypedArgument? next)
            : base(next)
        {
            this.Value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the value.
        /// </summary>
        [DataMember]
        public TArg Value { get; }

        #endregion

        #region Methods
        /// <summary>
        /// Gets the value.
        /// </summary>
        public sealed override object? GetValue()
        {
            return this.Value;
        }

        #endregion
    }
}
