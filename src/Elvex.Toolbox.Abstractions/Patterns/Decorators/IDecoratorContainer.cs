// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Abstractions.Patterns.Decorators
{
    public interface IDecoratorContainer<out TData>
    {
        TData Data { get; }
    }
}
