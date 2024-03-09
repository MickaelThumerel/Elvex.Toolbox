// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Extensions.Types
{
    using System.Threading.Tasks;

    /// <summary>
    /// Enhance the type <see cref="Task"/> or <see cref="Task{TResult}"/>
    /// </summary>
    /// <seealso cref="ITypeInfoExtensionEnhancer" />
    public interface ITaskTypeInfoEnhancer : ITypeInfoExtensionEnhancer
    {
        /// <summary>
        /// Gets the task result.
        /// </summary>
        object? GetResult(object? task);

        /// <summary>
        /// Gets result from <see cref="Task.FromResult"/> using the correct type
        /// </summary>
        Task GetTaskFromResult(object? resultInst);
    }
}
