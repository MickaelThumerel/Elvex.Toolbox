﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.WPF.ViewApp
{
    using Elvex.Toolbox.WPF.UI.Avalon.Services;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow(AvalonDockViewManager? avalonDockViewManager = null)
        {
            InitializeComponent();

            //this.avalonDock.Theme = new ToolBoxTheme();
            avalonDockViewManager?.RegisterDockManager(this.avalonDock);
        }
    }
}