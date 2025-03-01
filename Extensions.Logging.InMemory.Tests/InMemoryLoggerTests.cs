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
                    .Contains(loggedEntry =>
                                  loggedEntry.LogLevel == LogLevel.Information &&
                                  loggedEntry.EventId == 0 &&
                                  loggedEntry.FormattedMessage == "Test");
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
                    .Contains(new LoggedEntry(logLevel, eventId, message));
    }
}