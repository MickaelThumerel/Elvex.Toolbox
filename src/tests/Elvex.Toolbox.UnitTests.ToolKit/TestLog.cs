// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.UnitTests.ToolKit
{
    using Microsoft.Extensions.Logging;

    using System;

    public sealed record TestLog(LogLevel logLevel, EventId eventId, object? state, Exception? exception, string message);
}
