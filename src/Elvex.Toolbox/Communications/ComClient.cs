﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Communications
{
    using Elvex.Toolbox.Disposables;

    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="SafeAsyncDisposable" />
    internal sealed class ComClient : SafeAsyncDisposable, IObservable<byte[]>
    {
        #region Fields

        private readonly CancellationToken _token;

        private readonly TcpClient _tcpClient;
        private readonly Subject<byte[]> _subject;

        private readonly IDisposable _connectObservableToken;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ComClient"/> class.
        /// </summary>
        public ComClient(TcpClient tcpClient,
                         CancellationToken token)
        {
            this._tcpClient = tcpClient;
            this._token = token;

            this._subject = new Subject<byte[]>();
            var connectable = this._subject.SubscribeOn(TaskPoolScheduler.Default)
                                           .Publish();

            this._connectObservableToken = connectable.Connect();
            this.Proxy = new ComClientProxy(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        public ComClientProxy Proxy { get; }

        /// <summary>
        /// Gets the endpoint. (local or remote)
        /// </summary>
        public EndPoint? Endpoint
        {
            get { return this._tcpClient.Client?.LocalEndPoint ?? this._tcpClient.Client?.RemoteEndPoint; }
        }

        #endregion

        #region Events

        /// <summary>
        /// The client leave event
        /// </summary>
        public event Action<ComClient>? ClientLeaveEvent;

        #endregion

        #region Methods

        /// <summary>
        /// Starts listenning message
        /// </summary>
        public void Start()
        {
            Task.Run(RunListen).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        public void Send(byte[] data)
        {
            try
            {
                var quantitySize = sizeof(ushort);
                Span<byte> sizeBuffer = stackalloc byte[quantitySize + data.Length];

                var sizeArray = BitConverter.GetBytes((ushort)data.Length);
                sizeArray.CopyTo(sizeBuffer);
                data.CopyTo(sizeBuffer[quantitySize..]);

                var stream = this._tcpClient.GetStream();
                stream.Write(sizeBuffer);
            }
            catch (IOException)
            {
                ClientLeave();
            }
            catch (Exception)
            {
            }
        }

        /// <inheritdoc />
        public IDisposable Subscribe(IObserver<byte[]> observer)
        {
            return this._subject.Subscribe(observer);
        }

        #region Tools

        /// <summary>
        /// Loop to received messages
        /// </summary>
        private void RunListen()
        {
            using (var stream = this._tcpClient.GetStream())
            {
                var quantitySize = sizeof(ushort);
                Span<byte> sizeBuffer = stackalloc byte[quantitySize];

                try
                {
                    while (!this._token.IsCancellationRequested)
                    {
                        var sizeRead = stream.ReadAtLeast(sizeBuffer, quantitySize, true);

                        if (sizeRead != quantitySize)
                            break;

                        var size = BitConverter.ToUInt16(sizeBuffer);
                        sizeBuffer.Clear();

                        // TODO : Check max size limit
                        var messageBuffer = new byte[size];
                        stream.ReadExactly(messageBuffer, 0, messageBuffer.Length);

                        this._subject.OnNext(messageBuffer);
                    }
                }
                finally
                {
                    ClientLeave();
                    this._subject.OnCompleted();
                }
            }
        }

        /// <summary>
        /// Start to disposed
        /// </summary>
        protected override ValueTask DisposeBeginAsync()
        {
            this.Proxy.Dispose();
            this._tcpClient.Dispose();
            this._connectObservableToken.Dispose();

            return base.DisposeBeginAsync();
        }

        /// <summary>
        /// Clients the leave.
        /// </summary>
        private void ClientLeave()
        {
            ClientLeaveEvent?.Invoke(this);
        }

        #endregion

        #endregion
    }
}
