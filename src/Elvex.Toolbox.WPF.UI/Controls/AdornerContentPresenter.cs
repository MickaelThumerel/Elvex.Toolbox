// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.WPF.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    /// <summary>
    /// Adorner use to display content
    /// </summary>
    /// <seealso cref="System.Windows.Documents.Adorner" />
    public sealed class AdornerContentPresenter : Adorner
    {
        #region Fields

        private readonly VisualCollection _visuals;
        private readonly ContentPresenter _contentPresenter;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="AdornerContentPresenter"/> class.
        /// </summary>
        public AdornerContentPresenter(UIElement adornedElement)
          : base(adornedElement)
        {
            this._visuals = new VisualCollection(this);
            this._contentPresenter = new ContentPresenter()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };

            this._visuals.Add(this._contentPresenter);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdornerContentPresenter"/> class.
        /// </summary>
        public AdornerContentPresenter(UIElement adornedElement, Visual content)
          : this(adornedElement)
        {
            this.Content = content;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public object Content
        {
            get { return this._contentPresenter.Content; }
            set { this._contentPresenter.Content = value; }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        //protected override Size MeasureOverride(Size constraint)
        //{
        //    if (double.IsInfinity(constraint.Width) || double.IsInfinity(constraint.Height))
        //    {
        //        this.DesiredSize;
        //    }
        //    return constraint;
        //}

        /// <inheritdoc />
        protected override Size ArrangeOverride(Size finalSize)
        {
            this._contentPresenter.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return this._contentPresenter.RenderSize;
        }

        /// <inheritdoc />
        protected override Visual GetVisualChild(int index)
        {
            return this._visuals[index];
        }

        /// <inheritdoc />
        protected override int VisualChildrenCount
        {
            get { return this._visuals.Count; }
        }

        #endregion

    }
}
