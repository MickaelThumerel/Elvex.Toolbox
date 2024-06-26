﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox
{
    using Elvex.Toolbox.Models;

    using System.ComponentModel;

    /// <summary>
    /// Define a type used as none one
    /// </summary>
    [Serializable]
    [ImmutableObject(true)]
    public sealed class NoneType : IEquatable<NoneType>
    {
        #region Ctor

        /// <summary>
        /// Initializes the <see cref="NoneType"/> class.
        /// </summary>
        static NoneType()
        {
            Trait = typeof(NoneType);
            AbstractTrait = Trait.GetAbstractType();
            Instance = new NoneType();
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="NoneType"/> class from being created.
        /// </summary>
        private NoneType()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the trait.
        /// </summary>
        public static Type Trait { get; }

        /// <summary>
        /// Gets the trait.
        /// </summary>
        public static AbstractType AbstractTrait { get; }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static NoneType Instance { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether <typeparamref name="TType"/> is equal to <see cref="NoneType"/>.
        /// </summary>
        public static bool IsEqualTo<TType>()
        {
            return typeof(TType) == Trait;
        }

        /// <inheritdoc />
        public bool Equals(NoneType? other)
        {
            return other is not null;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is NoneType;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return 42;
        }

        #endregion
    }
}
