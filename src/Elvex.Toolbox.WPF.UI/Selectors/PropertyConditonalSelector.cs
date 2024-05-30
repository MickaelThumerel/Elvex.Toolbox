// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Elvex.Toolbox.WPF.UI.Selectors
{
    using Elvex.Toolbox.WPF.UI.Helpers;

    using System.Windows;

    /// <summary>
    /// Use a property and a binding to match
    /// </summary>
    public abstract class PropertyBaseConditonalSelector<TItem> : ConditonalBaseItem<TItem>, IConditionalSelector<TItem>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        public System.Windows.Data.Binding? Binding { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public object? Value { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override bool ValidateItemUsage(DependencyObject dependencyObject, object? dataContext)
        {
            if (this.Binding is null)
                return false;

            return ((dataContext is not null && object.Equals(BindingHelper.GetBindingResult(this.Binding, dataContext), this.Value)) ||
                    (dependencyObject is not null && object.Equals(BindingHelper.GetBindingResult(this.Binding, dependencyObject), this.Value)));
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class PropertyConditonalTemplateSelector : PropertyBaseConditonalSelector<DataTemplate>, IConditionalDataTemplate
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class PropertyConditonalStyleSelector : PropertyBaseConditonalSelector<Style>, IConditionalStyle
    {
    }
}
