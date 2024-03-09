﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.WPF.UI.Services
{
    using Elvex.Toolbox.Abstractions.Services;
    using Elvex.Toolbox.Extensions;
    using Elvex.Toolbox.WPF.Abstractions.Models;
    using Elvex.Toolbox.WPF.Abstractions.Services;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IRemoteExecHandler" />
    public sealed class RemoteExecHandler : IRemoteExecHandler
    {
        #region Fields

        private readonly IFileSystemHandler _fileSystemHandler;
        private readonly string _executableName;
        private readonly string _toolName;
        private readonly Uri _uri;

        private readonly string _installPath;
        private readonly Uri _execPathUri;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteExecHandler"/> class.
        /// </summary>
        public RemoteExecHandler(Uri uri,
                                 string toolName,
                                 string executableName,
                                 IFileSystemHandler fileSystemHandler)
        {
            this._uri = uri;
            this._toolName = toolName;
            this._executableName = executableName;
            this._fileSystemHandler = fileSystemHandler;

            var installPath = Path.Combine(Directory.GetCurrentDirectory(), ".tools", this._toolName);
            var execPathUri = new Uri(Path.Combine(installPath, this._executableName));

            this._installPath = installPath;
            this._execPathUri = execPathUri;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public async Task<RemoteExecResultRemoteExecResult> ExecuteAsync(CancellationToken token, params string[] arguments)
        {
            await InstallPkgAsync(token);

            var execPath = Path.Combine(this._installPath, this._executableName);

            var proc = new Process();
            string logs = string.Empty;
            string errors = string.Empty;
            Exception? exception = null;

            try
            {
                proc.StartInfo.FileName = execPath;
                proc.StartInfo.Arguments = string.Join(" ", arguments);

                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.RedirectStandardOutput = true;

                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;
                var consoleOutput = new Queue<string>();

                proc.ErrorDataReceived += (d, e) => consoleOutput.Enqueue("INFO:" + (e.Data ?? string.Empty));
                proc.OutputDataReceived += (d, e) => consoleOutput.Enqueue("ERROR:" + (e.Data ?? string.Empty));

                token.ThrowIfCancellationRequested();

                if (proc.Start())
                {
                    await proc.WaitForExitAsync(token);

                    logs = await proc.StandardOutput.ReadToEndAsync(token);
                    errors = await proc.StandardError.ReadToEndAsync(token);
                }

                token.ThrowIfCancellationRequested();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return new RemoteExecResultRemoteExecResult(proc.ExitCode == 0,
                                                        logs,
                                                        errors,
                                                        proc.ExitCode,
                                                        exception?.GetFullString() ?? string.Empty);
        }

        /// <summary>
        ///  Check if tool is installed otherwise install it.
        /// </summary>
        private async Task InstallPkgAsync(CancellationToken token)
        {
            if (this._fileSystemHandler.Exists(this._execPathUri))
                return;

            var bytes = await GetBytesFromUriAsync(this._uri);

            if (ExtractZipFileIfCan(bytes, this._installPath))
                return;

            //#warning TODO : Move file write to Toolbox IFileSystemHandler
            //            await File.WriteAllBytesAsync(this._execPathUri.OriginalString, bytes);
            await this._fileSystemHandler.WriteToFileAsync(bytes, this._execPathUri, @override: true, token: token);
        }

        /// <summary>
        /// Extracts the zip file if can.
        /// </summary>
        private bool ExtractZipFileIfCan(byte[] bytes, string installPath)
        {
            try
            {
                using (var stream = new MemoryStream(bytes))
                {
                    var zip = new ZipArchive(stream, ZipArchiveMode.Read);
                    var entries = zip.Entries;

                    zip.ExtractToDirectory(installPath);
                    return true;
                }
            }
            catch (Exception ex)
            {
#warning LOG ERROR
            }

            return false;
        }

        /// <summary>
        /// Gets the bytes from URI asynchronous.
        /// </summary>
        private async Task<byte[]> GetBytesFromUriAsync(Uri uri)
        {
            if (uri.IsFile)
            {
                using (var readStream = this._fileSystemHandler.OpenRead(uri)!)
                {
                    return readStream.ReadAll();
                }
            }

            using (var client = new HttpClient())
            {
                using (var result = await client.GetAsync(uri))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        return await result.Content.ReadAsByteArrayAsync();
                    }
                }
            }

            return EnumerableHelper<byte>.ReadOnlyArray;
        }

        #endregion
    }
}
