﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Extensions.Types
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Enhance the type <see cref="Task"/> or <see cref="Task{TResult}"/>
    /// </summary>
    /// <seealso cref="ITypeInfoExtensionEnhancer" />
    internal class TaskTypeInfoEnhancer : ITaskTypeInfoEnhancer
    {
        #region Methods

        /// <summary>
        /// Creates the specified <see cref="ITaskTypeInfoEnhancer"/>.
        /// </summary>
        public static ITaskTypeInfoEnhancer Create(Type trait)
        {
            var genericTask = trait.GetTreeValues(t => t?.BaseType)
                                   .FirstOrDefault(t => t != null && t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Task<>));

            if (genericTask != null)
            {
                var finalType = typeof(TaskTypeInfoEnhancer<>).MakeGenericType(genericTask.GetGenericArguments().First());

                // If finalType contains unresolved generic arg the fullname is null
                if (!string.IsNullOrEmpty(finalType.FullName))
                    return (ITaskTypeInfoEnhancer)Activator.CreateInstance(finalType)!;
            }

            return new TaskTypeInfoEnhancer();
        }

        /// <inheritdoc />
        public virtual object? GetResult(object? _)
        {
            return null;
        }

        /// <inheritdoc />
        public virtual Task GetTaskFromResult(object? resultInst)
        {
            if (resultInst is null)
                return Task.CompletedTask;

            return Task.FromResult(resultInst);
        }

        #endregion
    }

    /// <summary>
    /// Enhance the type <see cref="Task"/> or <see cref="Task{TResult}"/>
    /// </summary>
    /// <seealso cref="ITypeInfoExtensionEnhancer" />
    internal sealed class TaskTypeInfoEnhancer<TResult> : TaskTypeInfoEnhancer
    {
        /// <inheritdoc />
        public override object? GetResult(object? task)
        {
            if (task is null)
                return null;

            if (task is Task<TResult> taskResult)
                return taskResult.Result;

            throw new InvalidOperationException("MUST receied a Task<" + typeof(TResult) + "> but receved instead " + task);
        }

        /// <inheritdoc />
        public override Task GetTaskFromResult(object? resultInst)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            return Task.FromResult<TResult>((TResult?)resultInst);
#pragma warning restore CS8604 // Possible null reference argument.
        }
    }
}
