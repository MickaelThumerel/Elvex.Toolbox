﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Patterns.Messenger
{
    using Elvex.Toolbox.Abstractions.Patterns.Messenger;
    using Elvex.Toolbox.Abstractions.Proxies;
    using Elvex.Toolbox.Extensions;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public sealed class MessageQueueService : IMessageQueueService
    {
        #region Fields

        private readonly Dictionary<ChannelKey, IMessageQueueChannel> _channels;
        private readonly ReaderWriterLockSlim _locker;

        private readonly ILogger<IMessageQueueService> _logger;
        private readonly IDispatcherProxy? _dispatcherProxy;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueueService"/> class.
        /// </summary>
        public MessageQueueService(ILogger<IMessageQueueService>? logger = null,
                                   IDispatcherProxy? dispatcherProxy = null)
        {
            this._channels = new Dictionary<ChannelKey, IMessageQueueChannel>();
            this._locker = new ReaderWriterLockSlim();

            this._logger = logger ?? NullLogger<IMessageQueueService>.Instance;
            this._dispatcherProxy = dispatcherProxy;
        }

        #endregion

        #region Nested

        /// <summary>
        /// Complex key grouping <paramref name="SubchannelCategory"/> and <paramref name="MessageType"/>
        /// </summary>
        private sealed record class ChannelKey(Type MessageType, string? SubchannelCategory);

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Push<TMessage>(TMessage message, string? category = null)
            where TMessage : IMessage
        {
            Task.Run(async () =>
            {
                try
                {
                    await SendAsync(message, category);
                }
                catch (Exception ex)
                {
                    this._logger.OptiLog(LogLevel.Error, "Error during MessageQueue push message {message} - Category {category}", message, category);
                    this._dispatcherProxy?.Throw(ex, nameof(MessageQueueService) + "." + "Push<" + typeof(TMessage) + ">()");
                }
            }).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask SendAsync<TMessage>(TMessage message, string? category = null, CancellationToken token = default)
            where TMessage : IMessage
        {
            var channels = new MessageQueueChannel<TMessage>[2];

            var key = new ChannelKey(typeof(TMessage), category);

            this._locker.EnterReadLock();
            try
            {
                if (this._channels.TryGetValue(key, out var channel))
                    channels[0] = (MessageQueueChannel<TMessage>)channel;

                if (!string.IsNullOrEmpty(category))
                {
                    var parentKey = new ChannelKey(typeof(TMessage), null);
                    if (this._channels.TryGetValue(parentKey, out var parentChannel))
                        channels[1] = (MessageQueueChannel<TMessage>)parentChannel;
                }
            }
            finally
            {
                this._locker.ExitReadLock();
            }

            for (var i = 0; i < 2; i++)
            {
                var channel = channels[i];
                await (channel?.SendAsync(message, token) ?? ValueTask.CompletedTask);
            }
        }

        /// <inheritdoc />
        public IMessageQueueSubscription<TMessage> Subscribe<TMessage>(string? category = null) where TMessage : IMessage
        {
            this._locker.EnterWriteLock();
            try
            {
                var key = new ChannelKey(typeof(TMessage), category);
                IMessageQueueChannel? channel = null;

                if (!this._channels.TryGetValue(key, out channel))
                {
                    var parentKey = new ChannelKey(typeof(TMessage), null);

                    IMessageQueueChannel? parent = null;

                    if (key != parentKey && !string.IsNullOrEmpty(category))
                    {
                        if (!this._channels.TryGetValue(parentKey, out parent))
                        {
                            parent = new MessageQueueChannel<TMessage>();
                            this._channels.Add(parentKey, parent);
                        }
                    }

                    channel = new MessageQueueChannel<TMessage>((MessageQueueChannel<TMessage>)parent!, category);

                    this._channels.Add(key, channel);
                }

                Debug.Assert(channel is not null);

                return (MessageQueueChannel<TMessage>)channel!;

            }
            finally
            {
                this._locker.ExitWriteLock();
            }
        }

        #endregion
    }
}
