﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Models
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;

    /// <summary>
    /// <see cref="AbstractType"/> representing a collection
    /// </summary>
    /// <seealso cref="AbstractType" />
    [DataContract]
    [Serializable]
    [ImmutableObject(true)]
    public sealed class CollectionType : ConcretBaseType
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionType"/> class.
        /// </summary>
        [System.Text.Json.Serialization.JsonConstructor]
        [Newtonsoft.Json.JsonConstructor]
        public CollectionType(string displayName,
                              string? namespaceName,
                              string assemblyQualifiedName,
                              bool isInterface,
                              AbstractType itemAbstractType)
            : base(displayName,
                   namespaceName,
                   assemblyQualifiedName,
                   isInterface,
                   true,
                   AbstractTypeCategoryEnum.Collection)
        {
            this.ItemAbstractType = itemAbstractType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the item abstract.
        /// </summary>
        [DataMember]
        public AbstractType ItemAbstractType { get; }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override bool OnConcretEquals(ConcretBaseType otherConcret)
        {
            return otherConcret is CollectionType collection &&
                   (this.ItemAbstractType?.Equals(collection.ItemAbstractType) ?? collection.ItemAbstractType is null);
        }

        /// <inheritdoc />
        protected override object OnConcreteGetHashCode()
        {
            return this.ItemAbstractType.GetHashCode();
        }

        #endregion
    }
}
