﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Patterns.Tree
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Tree node
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class TreeNode<TEntity>
    {
        #region Fields

        private readonly List<TreeNode<TEntity>> _children;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{TEntity}"/> class.
        /// </summary>
        public TreeNode(TEntity entity, TreeNode<TEntity>? parent = null)
        {
            this._children = new List<TreeNode<TEntity>>();
            this.Parent = parent;
            this.Entity = entity;

            this.Parent?._children.Add(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the entity.
        /// </summary>
        public TEntity Entity { get; }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public TreeNode<TEntity>? Parent { get; }

        /// <summary>
        /// Gets the depth.
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public int Depth
        {
            get { return (this.Parent?.Depth ?? -1) + 1; }
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        public IReadOnlyCollection<TreeNode<TEntity>> Children
        {
            get { return this._children; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Attacheds the child.
        /// </summary>
        public void AttachedChild(TreeNode<TEntity> child)
        {
            ArgumentNullException.ThrowIfNull(child);

            if (child?.Parent != null)
                return;

            this._children.Add(child!);
            OnChildAdded(child);
        }

        /// <summary>
        /// Dettacheds a child.
        /// </summary>
        public void DettachedChild(TreeNode<TEntity> child)
        {
            ArgumentNullException.ThrowIfNull(child);

            if (this._children.Remove(child!))
                OnChildRemoved(child);
        }

        #region Tools

        /// <summary>
        /// Call each time a new child is added by the method <see cref="AttachedChild(TreeNode{TEntity})"/>
        /// </summary>
        protected virtual void OnChildAdded(TreeNode<TEntity>? child)
        {
        }

        /// <summary>
        /// Call each time a child is removed by the method <see cref="AttachedChild(TreeNode{TEntity})"/>
        /// </summary>
        protected virtual void OnChildRemoved(TreeNode<TEntity>? child)
        {
        }

        #endregion

        #endregion
    }
}
