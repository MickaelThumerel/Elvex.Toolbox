// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.ComServer.Abstraction.Test
{
    using Elvex.Toolbox.Abstractions.Attributes;

    [RefRoute("chat")]
    public sealed record class ChatMessage(string From, string Message, DateTime CreateTime);

    public sealed record class OtherClientsRequest();

    public sealed record class OtherClientsResponse(IReadOnlyCollection<string> Clients);
}
