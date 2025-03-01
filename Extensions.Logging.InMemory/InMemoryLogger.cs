using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Extensions.Logging.InMemory
{
    public class InMemoryLogger<T> : ILogger<T>
    {
        private readonly List<LoggedEntry> _entries = new();
        private readonly IExternalScopeProvider _scopeProvider = new LoggerExternalScopeProvider();

        public IReadOnlyCollection<LoggedEntry> LoggedEntries => _entries.AsReadOnly();

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            _entries.Add(new LoggedEntry(
                             logLevel,
                             eventId,
                             formatter(state, exception)));
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _scopeProvider.Push(state);
        }
    }
}