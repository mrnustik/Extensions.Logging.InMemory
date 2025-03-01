using System;
using System.Collections.Generic;
using System.Linq;
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
            var properties = (state as IReadOnlyList<KeyValuePair<string, object>>)
                             ?.ToDictionary(t => t.Key, t => t.Value)
                             ?? new Dictionary<string, object>();
            _entries.Add(new LoggedEntry(
                             logLevel,
                             eventId,
                             formatter(state, exception),
                             properties.GetValueOrDefault("{OriginalFormat}")?.ToString(),
                             properties));
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