using Microsoft.Extensions.Logging;

namespace Extensions.Logging.InMemory.Tests;

public class BasicTests
{
    [Test]
    public async Task LogEntries_WithNoLogOperations_IsEmpty()
    {
        // Arrange & Act
        var logger = new InMemoryLogger<BasicTests>();

        // Assert
        await Assert.That(logger.LoggedEntries)
                    .IsEmpty();
    }

    [Test]
    public async Task Log_WithBasicTextLoggedViaExtensionMethod_Should_BeAddedToLogEntries()
    {
        // Arrange
        var logger = new InMemoryLogger<BasicTests>();

        // Act
        logger.LogInformation("Test");

        // Assert
        await Assert.That(logger.LoggedEntries)
                    .HasSingleItem();

        using (Assert.Multiple())
        {
            var loggedEntry = logger.LoggedEntries.Single();
            await Assert.That(loggedEntry.LogLevel)
                        .IsEqualTo(LogLevel.Information);
            await Assert.That(loggedEntry.Message)
                        .IsEqualTo("Test");
        }
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
        var logger = new InMemoryLogger<BasicTests>();
        var eventId = 42;
        var message = "Test";

        // Act
        logger.Log(logLevel, eventId, message);

        // Assert
        await Assert.That(logger.LoggedEntries)
                    .HasSingleItem();
        using (Assert.Multiple())
        {
            var loggedEntry = logger.LoggedEntries.Single();
            await Assert.That(loggedEntry.LogLevel)
                        .IsEqualTo(logLevel);
            await Assert.That(loggedEntry.EventId)
                        .IsEqualTo(eventId);
            await Assert.That(loggedEntry.Message)
                        .IsEqualTo("Test");
        }
    }
}