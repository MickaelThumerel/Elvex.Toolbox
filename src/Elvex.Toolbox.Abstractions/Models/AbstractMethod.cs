﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Models
{
    using Elvex.Toolbox.Models;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>
    /// Information about a <see cref="MethodInfo"/>
    /// </summary>
    [DataContract]
    [Serializable]
    [ImmutableObject(true)]
    public sealed class AbstractMethod : IEquatable<AbstractMethod>, IEquatable<MethodInfo>
    {
        #region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractMethod"/> class.
        /// </summary>
        [System.Text.Json.Serialization.JsonConstructor]
        [Newtonsoft.Json.JsonConstructor]
        public AbstractMethod(string displayName,
                              string methodName,
                              string methodUniqueId,
                              bool isConstructor,
                              AbstractType? returnType,
                              IEnumerable<AbstractType> arguments,
                              IEnumerable<AbstractType> genericArguments)
        {
            this.DisplayName = displayName;
            this.MethodName = methodName;
            this.MethodUniqueId = methodUniqueId;
            this.ReturnType = returnType;
            this.Arguments = arguments?.ToArray() ?? Array.Empty<AbstractType>();
            this.HasArguments = this.Arguments.Any();
            this.GenericArguments = genericArguments?.ToArray() ?? Array.Empty<AbstractType>();
            this.HasGenericArguments = this.GenericArguments.Any();
            this.IsConstructor = isConstructor;
            this.IsIncomplet = this.Arguments.Any(a => a.Category == AbstractTypeCategoryEnum.Incomplet || 
                                                       a.Category == AbstractTypeCategoryEnum.Generic ||
                                                       a.Category == AbstractTypeCategoryEnum.GenericRef) ||
                               this.GenericArguments.Any(a => a.Category == AbstractTypeCategoryEnum.Incomplet ||
                                                              a.Category == AbstractTypeCategoryEnum.Generic ||
                                                              a.Category == AbstractTypeCategoryEnum.GenericRef);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the display name.
        /// </summary>
        [DataMember]
        public string DisplayName { get; }

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        [DataMember]
        public string MethodName { get; }

        /// <summary>
        /// Gets the method unique identifier.
        /// </summary>
        [DataMember]
        public string MethodUniqueId { get; }

        /// <summary>
        /// Gets the type of the return.
        /// </summary>
        [DataMember]
        public AbstractType? ReturnType { get; }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        [DataMember]
        public IReadOnlyCollection<AbstractType> Arguments { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has arguments.
        /// </summary>
        [DataMember]
        public bool HasArguments { get; }

        /// <summary>
        /// Gets the generic arguments.
        /// </summary>
        [DataMember]
        public IReadOnlyCollection<AbstractType> GenericArguments { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has generic arguments.
        /// </summary>
        [DataMember]
        public bool HasGenericArguments { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is constructor.
        /// </summary>
        [DataMember]
        public bool IsConstructor { get; }

        /// <summary>
        /// Gets a value indicating whether this method type is incomplet.
        /// </summary>
        [DataMember]
        public bool IsIncomplet { get; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public bool Equals(AbstractMethod? other)
        {
            if (other is null)
                return false;

            if (object.ReferenceEquals(this, other))
                return true;

            return this.DisplayName == other.DisplayName &&
                   this.MethodName == other.MethodName &&
                   this.MethodUniqueId == other.MethodUniqueId &&
                   this.ReturnType == other.ReturnType &&
                   this.HasArguments == other.HasArguments &&
                   this.HasGenericArguments == other.HasGenericArguments &&
                   this.Arguments.SequenceEqual(other.Arguments) &&
                   this.IsConstructor == other.IsConstructor &&
                   this.IsIncomplet == other.IsIncomplet && 
                   this.GenericArguments.SequenceEqual(other.GenericArguments);
        }

        /// <inheritdoc />
        public bool Equals(MethodInfo? other)
        {
            if (other is null)
                return false;

            return AbstractTypeExtensions.IsEqualTo(this, other);
        }

        /// <inheritdoc />
        public sealed override bool Equals(object? obj)
        {
            if (obj is AbstractMethod method)
                return Equals(method);

            if (obj is MethodInfo info)
                return Equals(info);

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(HashCode.Combine(this.DisplayName,
                                                     this.MethodName,
                                                     this.MethodUniqueId,
                                                     this.ReturnType,
                                                     this.HasArguments,
                                                     this.HasGenericArguments),
                                    this.IsConstructor,
                                    this.IsIncomplet,
                                    this.GenericArguments?.Aggregate(0, (acc, i) => acc ^ i.GetHashCode()),
                                    this.Arguments?.Aggregate(0, (acc, i) => acc ^ i.GetHashCode()));
        }

        /// <inheritdoc />
        public static bool operator ==(AbstractMethod? lhs, AbstractMethod? rhs)
        {
            return lhs?.Equals(rhs) ?? lhs is null;
        }

        /// <inheritdoc />
        public static bool operator !=(AbstractMethod? lhs, AbstractMethod? rhs)
        {
            return !(lhs == rhs);
        }

        /// <inheritdoc />
        public static bool operator ==(AbstractMethod? lhs, MethodInfo? rhs)
        {
            return lhs?.Equals(rhs) ?? lhs is null;
        }

        /// <inheritdoc />
        public static bool operator !=(AbstractMethod? lhs, MethodInfo? rhs)
        {
            return !(lhs == rhs);
        }

        #endregion
    }
}
