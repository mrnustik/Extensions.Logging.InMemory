using Microsoft.Extensions.Logging;

namespace Extensions.Logging.InMemory
{
    public record LoggedEntry(
        LogLevel LogLevel,
        EventId EventId,
        string FormattedMessage);
}