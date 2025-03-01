using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Extensions.Logging.InMemory
{
    public record LoggedEntry(
        LogLevel LogLevel,
        EventId EventId,
        string Message,
        string? OriginalFormat,
        Dictionary<string, object> Properties);
}