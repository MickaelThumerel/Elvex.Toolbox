﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Extensions
{
    using System;

    public static class StreamExtensions
    {
        #region Fields

        private const int ReadBatchSize = 0x008000;

        #endregion

        /// <summary>
        /// Reads all content bytes
        /// </summary>
        public static byte[] ReadAll(this Stream stream)
        {
            var bytes = new byte[stream.Length];

            var readed = 0;

            while (readed < stream.Length)
            {
                var remain = stream.Length - readed;
                int readLength = Math.Min((int)remain, ReadBatchSize);
                var readCount = stream.Read(bytes, readed, readLength);

                if (readCount < readLength)
                    break;

                readed += readCount;
            }

            return bytes;
        }
    }
}
