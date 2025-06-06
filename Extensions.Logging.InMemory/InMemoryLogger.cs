﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Extensions.Logging.InMemory
{
    /// <inheritdoc />
    public class InMemoryLogger<T> : ILogger<T>
    {
        private readonly List<LoggedEntry> _entries = new();
        private readonly IExternalScopeProvider _scopeProvider = new LoggerExternalScopeProvider();

        /// <summary>
        /// Readonly collection of logged entries
        /// </summary>
        public IReadOnlyCollection<LoggedEntry> LoggedEntries => _entries.AsReadOnly();

        /// <inheritdoc />
        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            var scopeProperties = CollectScopeProperties();
            var logProperties = CollectLogProperties(state);
            var properties = CombineProperties(scopeProperties, logProperties);
            _entries.Add(new LoggedEntry
            {
                LogLevel = logLevel,
                EventId = eventId,
                Message = formatter.Invoke(state, exception),
                Exception = exception,
                OriginalFormat = properties.GetValueOrDefault("{OriginalFormat}")?.ToString(),
                Properties = properties
            });
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        /// <inheritdoc />
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
        {
            return _scopeProvider.Push(state);
        }

        private static Dictionary<string, object> CombineProperties(Dictionary<string, object> scopeProperties,
                                                                    Dictionary<string, object> logProperties)
        {
            var properties = new Dictionary<string, object>(scopeProperties);

            foreach (var property in logProperties)
                properties[property.Key] = property.Value;
            return properties;
        }

        private static Dictionary<string, object> CollectLogProperties<TState>(TState state)
        {
            var logProperties = (state as IEnumerable<KeyValuePair<string, object>>)
                                ?.ToDictionary(t => t.Key, t => t.Value)
                                ?? new Dictionary<string, object>();
            return logProperties;
        }

        private Dictionary<string, object> CollectScopeProperties()
        {
            var scopeProperties = new Dictionary<string, object>();
            _scopeProvider.ForEachScope((scope, state) =>
            {
                if (scope is not IEnumerable<KeyValuePair<string, object>> propertyBasedScope)
                    return;

                foreach (var property in propertyBasedScope)
                {
                    state[property.Key] = property.Value;
                }
            }, scopeProperties);
            return scopeProperties;
        }
    }
}