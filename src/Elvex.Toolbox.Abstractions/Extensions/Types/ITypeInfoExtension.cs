﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Extensions.Types
{
    /// <summary>
    /// Extend type information
    /// </summary>
    public interface ITypeInfoExtension
    {
        #region Properties

        /// <summary>
        /// Gets the trait.
        /// </summary>
        Type Trait { get; }

        /// <summary>
        /// Determines whether this <see cref="Type"/> is a collection type.
        /// </summary>
        bool IsCollection { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is c sharp scalar type.
        /// </summary>
        bool IsCSharpScalarType { get; }

        /// <summary>
        /// Gets the type of the item collection.
        /// </summary>
        Type? CollectionItemType { get; }

        /// <summary>
        /// Gets the default value
        /// </summary>
        /// <remarks>
        ///     May be box
        /// </remarks>
        object? Default { get; }

        /// <summary>
        /// Gets the full name of the short.
        /// </summary>
        string FullShortName { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is a <see cref="ValueTask"/> or <see cref="ValueTask{TResult}"/>.
        /// </summary>
        bool IsValueTask { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is a task.
        /// </summary>
        bool IsTask { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a <see cref="ITypeInfoExtensionEnhancer"/> attached to this type information
        /// </summary>
        TTypeInfoExtensionEnhancer GetSpecifcTypeExtend<TTypeInfoExtensionEnhancer>()
            where TTypeInfoExtensionEnhancer : ITypeInfoExtensionEnhancer;

        /// <summary>
        /// Gets all compatible types (Parent class, Interfaces, ...)
        /// </summary>
        IReadOnlyCollection<Type> GetAllCompatibleTypes();

        #endregion
    }
}
