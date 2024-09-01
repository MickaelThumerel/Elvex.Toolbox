// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.WPF.ViewApp.ViewModels
{
    using Elvex.Toolbox.Abstractions.Proxies;

    public sealed class MainViewModel : BaseViewModel
    {
        public MainViewModel(IDispatcherProxy dispatcherProxy) : base(dispatcherProxy)
        {
            this.Templates = Enum.GetValues<TemplateSelectorEnum>();
        }

        public IReadOnlyCollection<TemplateSelectorEnum> Templates { get; }
    }
}
