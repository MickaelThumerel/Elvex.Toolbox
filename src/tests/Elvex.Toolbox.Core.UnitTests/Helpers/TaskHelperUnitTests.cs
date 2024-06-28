// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Core.UnitTests.Helpers
{
    using Elvex.Toolbox.Abstractions.Helpers;

    using NFluent;

    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Test for <see cref="TaskHelper"/>
    /// </summary>
    public sealed class TaskHelperUnitTests
    {
        [Fact]  
        public void FixedCache()
        {
            string testHost = Guid.NewGuid().ToString();

            var classicTask = Task.FromResult(testHost);
            var classicTaskBis = Task.FromResult(testHost);

            Check.That(classicTask).IsNotNull().And.Not.IsSameReferenceAs(classicTaskBis);

            var cachedTsk = TaskHelper.GetFromResultCache(testHost);
            var cachedTskBis = TaskHelper.GetFromResultCache(testHost);

            Check.That(cachedTsk).IsNotNull().And.IsSameReferenceAs(cachedTskBis);
        }

        [Fact]
        public void FixedCache_Clear()
        {
            string testHost = Guid.NewGuid().ToString();

            var cachedTsk = TaskHelper.GetFromResultCache(testHost);
            var cachedTskBis = TaskHelper.GetFromResultCache(testHost);

            Check.That(cachedTsk).IsNotNull().And.IsSameReferenceAs(cachedTskBis);

            TaskHelper.ClearFromResultCache(testHost);
            var cachedTskTrd = TaskHelper.GetFromResultCache(testHost);

            Check.That(cachedTsk).IsNotNull().And.Not.IsSameReferenceAs(cachedTskTrd);
        }
    }
}
