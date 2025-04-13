using Microsoft.Extensions.Logging;

namespace Extensions.Logging.InMemory.Tests;

public class BasicTests
{
    [Test]
    public async Task LogEntries_WithNoLogOperations_ShouldBeEmpty()
    {
        // Arrange & Act
        var logger = new InMemoryLogger<BasicTests>();

        // Assert
        await Assert.That(logger.LoggedEntries)
            .IsEmpty();
    }

    [Test]
    public async Task Log_WithBasicTextLoggedViaExtensionMethod_ShouldBeAddedToLogEntries()
    {
        // Arrange
        var logger = new InMemoryLogger<BasicTests>();

        // Act
        logger.LogInformation("Test");

        // Assert
        await Assert.That(logger.LoggedEntries)
            .HasSingleItem();
        var loggedEntry = logger.LoggedEntries.Single();
        await Assert.That(loggedEntry)
            .IsPartiallyEquivalentTo(new
            {
                LogLevel = LogLevel.Information,
                Message = "Test"
            });
    }

    [Test]
    [Arguments(LogLevel.Trace)]
    [Arguments(LogLevel.Debug)]
    [Arguments(LogLevel.Information)]
    [Arguments(LogLevel.Warning)]
    [Arguments(LogLevel.Error)]
    [Arguments(LogLevel.Critical)]
    public async Task Log_WithSpecificLogLevel_ShouldHaveExpectedLogLevel(LogLevel logLevel)
    {
        // Arrange
        var logger = new InMemoryLogger<BasicTests>();
        EventId eventId = 42;
        var message = "Test";

        // Act
        logger.Log(logLevel, eventId, message);

        // Assert
        await Assert.That(logger.LoggedEntries)
            .HasSingleItem();
        var loggedEntry = logger.LoggedEntries.Single();
        await Assert.That(loggedEntry)
            .IsPartiallyEquivalentTo(new
            {
                LogLevel = logLevel,
                EventId = eventId,
                Message = message
            });
    }

    [Test]
    public async Task Log_WithException_ShouldHaveException()
    {
        // Arrange
        var logger = new InMemoryLogger<BasicTests>();
        var message = "Message";
        var exception = new Exception("Test");

        // Act
        logger.LogInformation(exception, message);

        // Assert
        await Assert.That(logger.LoggedEntries)
            .HasSingleItem();
        var loggedEntry = logger.LoggedEntries.Single();
        await Assert.That(loggedEntry)
            .IsPartiallyEquivalentTo(new
            {
                LogLevel = LogLevel.Information,
                Message = message,
                Exception = exception
            });
    }
}