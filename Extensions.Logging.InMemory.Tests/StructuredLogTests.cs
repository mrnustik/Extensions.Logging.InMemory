using Microsoft.Extensions.Logging;

namespace Extensions.Logging.InMemory.Tests;

public class StructuredLogTests
{
    [Test]
    public async Task Log_WithStructuredLog_EntryHasFormattedMessage()
    {
        // Arrange
        var logger = new InMemoryLogger<StructuredLogTests>();
        var intValue = 42;
        var stringValue = "Test";

        // Act
        logger.LogInformation("Tested structured log message {String} {Int}", stringValue, intValue);

        // Assert
        await Assert.That(logger.LoggedEntries)
            .HasSingleItem();
        var loggedEntry = logger.LoggedEntries.Single();
        await Assert.That(loggedEntry)
            .IsPartiallyEquivalentTo(new
            {
                LogLevel = LogLevel.Information,
                Message = "Tested structured log message Test 42"
            });
    }

    [Test]
    public async Task Log_WithStructuredLog_EntryHasOriginalFormat()
    {
        // Arrange
        var logger = new InMemoryLogger<StructuredLogTests>();
        var intValue = 42;
        var stringValue = "Test";

        // Act
        logger.LogInformation("Tested structured log message {String} {Int}", stringValue, intValue);

        // Assert
        await Assert.That(logger.LoggedEntries)
            .HasSingleItem();
        var loggedEntry = logger.LoggedEntries.Single();
        await Assert.That(loggedEntry)
            .IsPartiallyEquivalentTo(new
            {
                LogLevel = LogLevel.Information,
                OriginalFormat = "Tested structured log message {String} {Int}"
            });
    }

    [Test]
    public async Task Log_WithBasicTextLog_EntryHasOriginalFormat()
    {
        // Arrange
        var logger = new InMemoryLogger<StructuredLogTests>();

        // Act
        logger.LogInformation("Test Log");

        // Assert
        await Assert.That(logger.LoggedEntries)
            .HasSingleItem();
        var loggedEntry = logger.LoggedEntries.Single();
        await Assert.That(loggedEntry)
            .IsPartiallyEquivalentTo(new
            {
                LogLevel = LogLevel.Information,
                OriginalFormat = "Test Log"
            });
    }

    [Test]
    public async Task Log_WithStructuredLogWithPrimitiveTypes_EntryHasLoggedProperties()
    {
        // Arrange
        var logger = new InMemoryLogger<StructuredLogTests>();
        var intValue = 42;
        var stringValue = "Test";

        // Act
        logger.LogInformation("Tested structured log message {String} {Int}", stringValue, intValue);

        // Assert
        await Assert.That(logger.LoggedEntries)
            .HasSingleItem();
        var loggedEntry = logger.LoggedEntries.Single();
        await Assert.That(loggedEntry)
            .IsPartiallyEquivalentTo(
                new
                {
                    LogLevel = LogLevel.Information,
                    Properties = new Dictionary<string, object>
                    {
                        { "String", stringValue },
                        { "Int", intValue },
                        { "{OriginalFormat}", "Tested structured log message {String} {Int}" }
                    }
                });
    }

    [Test]
    public async Task Log_WithStructuredLogWithDecomposedObject_EntryHasLoggedProperties()
    {
        // Arrange
        var logger = new InMemoryLogger<StructuredLogTests>();
        var intValue = 42;
        var stringValue = "Test";
        var decomposableObject = new DecomposableObject(stringValue, intValue);

        // Act
        logger.LogInformation("Tested structured log message {@DecomposableObject}", decomposableObject);

        // Assert
        await Assert.That(logger.LoggedEntries)
            .HasSingleItem();
        var loggedEntry = logger.LoggedEntries.Single();
        await Assert.That(loggedEntry)
            .IsPartiallyEquivalentTo(new
            {
                LogLevel = LogLevel.Information,
                Properties = new Dictionary<string, object>
                {
                    { "@DecomposableObject", decomposableObject },
                    { "{OriginalFormat}", "Tested structured log message {@DecomposableObject}" }
                }
            });
    }

    [Test]
    public async Task Log_WithStructuredLogWithFormattedPrimitive_EntryHasLoggedProperties()
    {
        // Arrange
        var logger = new InMemoryLogger<StructuredLogTests>();
        var dateTime = new DateTime(2000, 1, 1);

        // Act
        logger.LogInformation("Tested structured log message {DateTime:yyyy}", dateTime);

        // Assert
        await Assert.That(logger.LoggedEntries)
            .HasSingleItem();
        var loggedEntry = logger.LoggedEntries.Single();
        await Assert.That(loggedEntry)
            .IsPartiallyEquivalentTo(new
            {
                LogLevel = LogLevel.Information,
                Message = "Tested structured log message 2000",
                OriginalFormat = "Tested structured log message {DateTime:yyyy}",
                Properties = new Dictionary<string, object>
                {
                    { "DateTime", dateTime },
                    { "{OriginalFormat}", "Tested structured log message {DateTime:yyyy}" }
                }
            });
    }

    [Test]
    public async Task Log_WithSourceGeneratedLogs_EntryHasLoggedProperties()
    {
        // Arrange
        var logger = new InMemoryLogger<StructuredLogTests>();

        // Act
        logger.LogSomething("Something", 666);

        // Assert
        await Assert
            .That(logger.LoggedEntries)
            .HasSingleItem();
        var loggedEntry = logger.LoggedEntries.Single();
        await Assert.That(loggedEntry)
            .IsPartiallyEquivalentTo(new
            {
                LogLevel = LogLevel.Information,
                EventId = (EventId)42,
                OriginalFormat = "Tested structured log message {StringValue} {IntValue}",
                Properties = new Dictionary<string, object>()
                {
                    { "StringValue", "Something" },
                    { "IntValue", 666 },
                    { "{OriginalFormat}", "Tested structured log message {StringValue} {IntValue}" }
                }
            });
    }

    public record DecomposableObject(string String, int Int);
}

public static partial class LogExtensions
{
    [LoggerMessage(
        EventId = 42,
        Level = LogLevel.Information,
        Message = "Tested structured log message {StringValue} {IntValue}")]
    public static partial void LogSomething(this ILogger logger, string stringValue, int intValue);
}