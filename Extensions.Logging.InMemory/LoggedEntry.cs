using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Extensions.Logging.InMemory
{
    /// <summary>
    /// Represents a log entry containing detailed information about the log event,
    /// including the log level, event ID, message, exception, original format string,
    /// and additional properties.
    /// </summary>
    /// <param name="LogLevel">
    /// The severity level of the log entry.
    /// </param>
    /// <param name="EventId">
    /// The event ID associated with the log entry.
    /// </param>
    /// <param name="Message">
    /// The formatted log message generated for the entry.
    /// </param>
    /// <param name="Exception">
    /// An optional exception associated with the log entry. If no exception occurred, this value may be null.
    /// </param>
    /// <param name="OriginalFormat">
    /// The original message template format string used to generate the log message.
    /// </param>
    /// <param name="Properties">
    /// A collection of key-value pairs containing additional contextual information or
    /// metadata related to the log entry. These may include scope and log-specific properties.
    /// </param>
    public record LoggedEntry(
        LogLevel LogLevel,
        EventId EventId,
        string Message,
        Exception? Exception,
        string? OriginalFormat,
        Dictionary<string, object> Properties);
}