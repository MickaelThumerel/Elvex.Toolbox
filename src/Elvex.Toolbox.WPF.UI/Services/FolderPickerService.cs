﻿// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Elvex.Toolbox.WPF.UI.Services
{
    using Elvex.Toolbox.WPF.Abstractions.Services;

    using Microsoft.Win32;

    using System.Threading.Tasks;

    public sealed class FolderPickerService : IFolderPickerService
    {
        #region Methods

        /// <inheritdoc />
        public ValueTask<string?> PickFolderPathAsync(string? root = null)
        {
            var openFolderDialog = new OpenFolderDialog()
            {
                InitialDirectory = root,
            };

            if (openFolderDialog.ShowDialog() == true)
                return ValueTask.FromResult((string?)openFolderDialog.FolderName);

            return ValueTask.FromResult((string?)null);
        }

        #endregion
    }
}
