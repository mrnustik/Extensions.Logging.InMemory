using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Extensions.Logging.InMemory
{
    /// <summary>
    /// Represents a log entry containing detailed information about the logged event.
    /// </summary>
    public record LoggedEntry
    {
        /// <summary>
        /// The severity level of the log entry.
        /// </summary>
        public required LogLevel LogLevel { get; init; }

        /// <summary>
        /// The event ID associated with the log entry.
        /// </summary>
        public EventId EventId { get; init; }

        /// <summary>
        /// The formatted log message generated for the entry.
        /// </summary>
        public required string Message { get; init; }

        /// <summary>
        /// An optional exception associated with the log entry. If no exception occurred, this value may be null.
        /// </summary>
        public Exception? Exception { get; init; }

        /// <summary>
        /// The original message template format string used to generate the log message.
        /// </summary>
        public string? OriginalFormat { get; init; }

        /// <summary>
        /// A collection of key-value pairs containing additional contextual information or
        /// metadata related to the log entry. These may include scope and log-specific properties.
        /// </summary>
        public Dictionary<string, object> Properties { get; init; } = new();
    }
}