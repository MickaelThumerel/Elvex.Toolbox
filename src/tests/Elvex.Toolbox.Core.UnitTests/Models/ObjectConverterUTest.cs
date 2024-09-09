// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Core.UnitTests.Models
{
    using AutoFixture;

    using Elvex.Toolbox.Abstractions.Models;
    using Elvex.Toolbox.Abstractions.Supports;
    using Elvex.Toolbox.Models;

    using NFluent;

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Test for <see cref="ObjectConverter"/>
    /// </summary>
    public sealed class ObjectConverterUTest
    {
        #region Nested

        internal abstract record class Parent(string Id);

        internal sealed record class Child(string Id, string ChildId) : Parent(Id);

        internal sealed record class Source<TArg>(TArg item) : ISupportConvert
        {
            /// <inheritdoc />
            public bool TryConvert<TTarget>(out TTarget? target)
            {
                if (TryConvert(out var objTarget, typeof(TTarget)))
                {
                    target = (TTarget)objTarget;
                    return true;
                }

                target = default;
                return false;
            }

            /// <inheritdoc />
            public bool TryConvert(out object target, Type targetType)
            {
                var genericLocal = GetType().GetGenericTypeDefinition();
                if (targetType.IsGenericType && 
                    targetType.GetGenericTypeDefinition() == genericLocal &&
                    typeof(TArg).IsAssignableTo(targetType.GetGenericArguments().Single()))
                {
                    target = Activator.CreateInstance(targetType, new object?[] { this.item })!;
                    return true;
                }
                
                target = null;
                return false;
            }
        }

        #endregion

        #region Methods

        [Fact]
        public void Support_Convert()
        {
            var fixture = new Fixture();
            var item = new Source<Child>(new Child(fixture.Create<string>(), fixture.Create<string>()));

            var objConverter = new ObjectConverter(EnumerableHelper<IDedicatedObjectConverter>.Enumerable);

            var cnvGeneric = objConverter.TryConvert<Source<Parent>>(item, out var resultWithGeneric);

            Check.That(cnvGeneric).IsTrue();
            Check.That(resultWithGeneric).IsNotNull();
            Check.That(resultWithGeneric!.item).IsSameReferenceAs(item.item);

            var cnvDirect = objConverter.TryConvert(item, typeof(Source<Parent>), out var resultWithDirect);

            Check.That(cnvDirect).IsTrue();
            Check.That(resultWithDirect).IsNotNull().And.IsInstanceOf<Source<Parent>>();
            Check.That(((Source<Parent>)resultWithDirect!).item).IsSameReferenceAs(item.item);
        }

        #endregion
    }
}
