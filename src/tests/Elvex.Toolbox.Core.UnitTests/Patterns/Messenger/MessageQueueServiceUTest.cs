﻿// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Core.UnitTests.Patterns.Messenger
{
    using AutoFixture;

    using Elvex.Toolbox.Abstractions.Patterns.Messenger;
    using Elvex.Toolbox.Helpers;
    using Elvex.Toolbox.Patterns.Messenger;
    using Elvex.Toolbox.UnitTests.ToolKit.Helpers;

    using NFluent;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Test for <see cref="MessageQueueService"/>
    /// </summary>
    public sealed class MessageQueueServiceUTest
    {
        #region Fields
        
        private static readonly TimeSpan s_timeout;

        #endregion

        #region Nested

        private interface IMessageWithCategory
        {
            string? Category { get; }
        }

        private record class SimpleMessage(Guid Uid, string Text) : IMessage;

        private record class MessageWithCategory(Guid Uid, string Text, string? Category) : IMessage, IMessageWithCategory;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueueServiceUTest"/> class.
        /// </summary>
        static MessageQueueServiceUTest()
        {
            s_timeout = TimeSpan.FromSeconds(Debugger.IsAttached ? 1000 : 20);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Simples test, received eveythink sended
        /// </summary>
        [Fact]
        public async Task Simple()
        {
            using (var timeout = CancellationHelper.DisposableTimeout(s_timeout))
            {
                var fixture = ObjectTestHelper.PrepareFixture();

                var msgs = fixture.CreateMany<SimpleMessage>().ToReadOnly();

                await MessengerService_Tester_Impl(null, msgs, msgs, timeout.Content);
            }
        }

        /// <summary>
        /// Simples test, received eveythink sended
        /// </summary>
        [Fact]
        public async Task Simple_With_Category()
        {
            using (var timeout = CancellationHelper.DisposableTimeout(s_timeout))
            {
                var fixture = ObjectTestHelper.PrepareFixture();

                var msgs = fixture.CreateMany<SimpleMessage>().ToReadOnly();

                await MessengerService_Tester_Impl("Poney", msgs, msgs, timeout.Content);
            }
        }

        /// <summary>
        /// Simples test, received eveythink sended
        /// </summary>
        [Fact]
        public async Task Simple_With_Filter()
        {
            using (var timeout = CancellationHelper.DisposableTimeout(s_timeout))
            {
                var msgs = new[]
                {
                    new SimpleMessage(Guid.NewGuid(), "Poney Rose"),
                    new SimpleMessage(Guid.NewGuid(), "Mth"),
                    new SimpleMessage(Guid.NewGuid(), "Poney Rose"),
                };

                Func<SimpleMessage, string?, bool> predicate = (m, c) => m.Text.Contains("PONEY", StringComparison.OrdinalIgnoreCase);

                var expected = msgs.Where(m => predicate(m, "")).ToArray();

                Check.That(expected.Length).IsEqualTo(2);

                await MessengerService_Tester_Impl(null,
                                                   msgs,
                                                   expected,
                                                   timeout.Content,
                                                   predicate);
            }
        }


        /// <summary>
        /// Simples test, received eveythink sended
        /// </summary>
        [Fact]
        public async Task Categories()
        {
            using (var timeout = CancellationHelper.DisposableTimeout(s_timeout))
            {
                var fixture = ObjectTestHelper.PrepareFixture();

                var expectedCategory = "PONEY";

                var msgs = new[]
                {
                    new MessageWithCategory(Guid.NewGuid(), fixture.Create<string>(), fixture.Create<string>()),
                    new MessageWithCategory(Guid.NewGuid(), fixture.Create<string>(), expectedCategory),
                    new MessageWithCategory(Guid.NewGuid(), fixture.Create<string>(), fixture.Create<string>()),
                    new MessageWithCategory(Guid.NewGuid(), fixture.Create<string>(), expectedCategory),
                };

                var expected = msgs.Where(m => m.Category == expectedCategory).ToArray();

                Check.That(expected.Length).IsEqualTo(2);

                await MessengerService_Tester_Impl(expectedCategory, msgs, expected, timeout.Content);
            }
        }


        /// <summary>
        /// Messengers the service tester implementation.
        /// </summary>
        private async Task MessengerService_Tester_Impl<TMesssage>(string? category,
                                                                   IReadOnlyCollection<TMesssage> messageToSend,
                                                                   IReadOnlyCollection<TMesssage> expectedMessageToBeReceived,
                                                                   CancellationToken token,
                                                                   Func<TMesssage, string?, bool>? predicate = null)
            where TMesssage : IMessage, IEquatable<TMesssage>
        {
            using (var service = new MessageQueueService())
            {
                var messageReceived = new List<TMesssage>();

                var subToken = service.Subscribe<TMesssage>(category).Message(this, (msg, category) =>
                {
                    lock (messageReceived)
                    {
                        messageReceived.Add(msg);
                        token.ThrowIfCancellationRequested();
                    }
                    return ValueTask.CompletedTask;
                }, predicate);

                foreach (var msg in messageToSend)
                {
                    var pushCategory = category;

                    if (msg is IMessageWithCategory ctg)
                        pushCategory = ctg.Category;

                    service.Push<TMesssage>(msg, pushCategory);
                }

                while (true)
                {
                    if (messageReceived.Count >= expectedMessageToBeReceived.Count)
                    {
                        // Wait to allow error message to arrived
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1000);
                        break;
                    }

                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100);
                }

                // unsubscribe
                subToken.Dispose();

                token.ThrowIfCancellationRequested();

                // Send again to ensure subscription have been correctly removed
                foreach (var msg in messageToSend)
                    service.Push<TMesssage>(msg, category);

                // Wait to allow at least on error message to arrived
                token.ThrowIfCancellationRequested();
                await Task.Delay(1000);

                var results = messageReceived.GroupBy(m => m.Uid)
                                             .ToDictionary(k => k.Key, v => v.ToArray());

                Check.WithCustomMessage("Same message received multiple times")
                     .ThatCode(() => results.Any(r => r.Value.Length > 1))
                     .WhichResult().IsFalse();

                var indexedExpectedResult = expectedMessageToBeReceived.ToDictionary(k => k.Uid);

                Check.WithCustomMessage("Message number received differ from expected")
                     .That(results.Count).IsEqualTo(indexedExpectedResult.Count);

                foreach (var msg in indexedExpectedResult)
                {
                    if (results.TryGetValue(msg.Key, out var result))
                    {
                        token.ThrowIfCancellationRequested();
                        var r = result.First();
                        Check.That(r).IsEqualTo(msg.Value);

                        continue;
                    }

                    throw new InvalidDataException("Expected msg not found in the results");
                }
            }
        }

        #endregion
    }
}
