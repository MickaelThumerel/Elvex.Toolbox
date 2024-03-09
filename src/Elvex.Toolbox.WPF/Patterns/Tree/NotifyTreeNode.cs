﻿// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Elvex.Toolbox.WPF.Patterns.Tree
{
    using Elvex.Toolbox.Abstractions.Proxies;
    using Elvex.Toolbox.Patterns.Tree;

    using System.Collections.Specialized;

    /// <summary>
    /// Tree Node that implement the <see cref="INotifyCollectionChanged"/>
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="TreeNode{TEntity}" />
    /// <seealso cref="INotifyCollectionChanged" />
    public class NotifyTreeNode<TEntity> : TreeNode<TEntity>, INotifyCollectionChanged
    {
        #region Fields
        
        private readonly IDispatcherProxy _dispatcherProxy;
        
        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyTreeNode{TEntity}"/> class.
        /// </summary>
        public NotifyTreeNode(IDispatcherProxy dispatcherProxy, TEntity entity, TreeNode<TEntity>? parent = null) 
            : base(entity, parent)
        {
            this._dispatcherProxy = dispatcherProxy;
        }

        #endregion

        #region Events

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnChildAdded(TreeNode<TEntity>? child)
        {
            this._dispatcherProxy.Send(() =>
            {
                this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, child));
            });
            base.OnChildAdded(child);
        }

        /// <inheritdoc />
        protected override void OnChildRemoved(TreeNode<TEntity>? child)
        {
            this._dispatcherProxy.Send(() =>
            {
                this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, child));
            });

            base.OnChildRemoved(child);
        }

        #endregion
    }
}
