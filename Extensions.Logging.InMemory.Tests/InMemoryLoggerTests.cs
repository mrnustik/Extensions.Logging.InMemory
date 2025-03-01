using Microsoft.Extensions.Logging;

namespace Extensions.Logging.InMemory.Tests;

public class InMemoryLoggerTests
{
    [Test]
    public async Task BasicTextLog_LoggedViaExtensionMethod_Should_BeAddedToLogEntries()
    {
        // Arrange
        var logger = new InMemoryLogger<InMemoryLoggerTests>();

        // Act
        logger.LogInformation("Test");

        // Assert
        await Assert.That(logger.LoggedEntries)
                    .HasSingleItem()
                    .And
                    .Contains(entry =>
                                  entry.LogLevel == LogLevel.Information &&
                                  entry.EventId == 0 &&
                                  entry.FormattedMessage == "Test");
    }

    [Test]
    [Arguments(LogLevel.Trace)]
    [Arguments(LogLevel.Debug)]
    [Arguments(LogLevel.Information)]
    [Arguments(LogLevel.Warning)]
    [Arguments(LogLevel.Error)]
    [Arguments(LogLevel.Critical)]
    public async Task LogViaDefaultLogMethod_Should_HaveExpectedLogEntries(LogLevel logLevel)
    {
        // Arrange
        var logger = new InMemoryLogger<InMemoryLoggerTests>();
        var eventId = 42;
        var message = "Test";

        // Act
        logger.Log(logLevel, eventId, message);

        // Assert
        await Assert.That(logger.LoggedEntries)
                    .HasSingleItem()
                    .And
                    .Contains(entry =>
                                  entry.LogLevel == logLevel &&
                                  entry.EventId == eventId &&
                                  entry.FormattedMessage == message);
    }

    [Test]
    public async Task Log_WithStructuredLog_EntryHasFormattedMessage()
    {
        // Arrange
        var logger = new InMemoryLogger<InMemoryLoggerTests>();
        var intValue = 42;
        var stringValue = "Test";

        // Act
        logger.LogInformation("Tested structured log message {String} {Int}", stringValue, intValue);

        // Assert
        await Assert.That(logger.LoggedEntries)
                    .HasSingleItem()
                    .And
                    .Contains(entry =>
                                  entry.LogLevel == LogLevel.Information &&
                                  entry.FormattedMessage == "Tested structured log message Test 42");
    }

    [Test]
    public async Task Log_WithStructuredLog_EntryHasOriginalFormat()
    {
        // Arrange
        var logger = new InMemoryLogger<InMemoryLoggerTests>();
        var intValue = 42;
        var stringValue = "Test";

        // Act
        logger.LogInformation("Tested structured log message {String} {Int}", stringValue, intValue);

        // Assert
        await Assert.That(logger.LoggedEntries)
                    .HasSingleItem()
                    .And
                    .Contains(entry =>
                                  entry.LogLevel == LogLevel.Information &&
                                  entry.OriginalFormat == "Tested structured log message {String} {Int}");
    }

    [Test]
    public async Task Log_WithBasicTextLog_EntryHasNullOriginalFormat()
    {
        // Arrange
        var logger = new InMemoryLogger<InMemoryLoggerTests>();

        // Act
        logger.LogInformation("Test Log");

        // Assert
        await Assert.That(logger.LoggedEntries)
                    .HasSingleItem()
                    .And
                    .Contains(entry =>
                                  entry.LogLevel == LogLevel.Information &&
                                  entry.OriginalFormat == null);
    }
}