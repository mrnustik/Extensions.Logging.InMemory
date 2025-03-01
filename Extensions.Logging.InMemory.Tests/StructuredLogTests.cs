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
        using (Assert.Multiple())
        {
            var loggedEntry = logger.LoggedEntries.Single();
            await Assert.That(loggedEntry.LogLevel)
                        .IsEqualTo(LogLevel.Information);
            await Assert.That(loggedEntry.Message)
                        .IsEqualTo("Tested structured log message Test 42");
        }
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
        using (Assert.Multiple())
        {
            var loggedEntry = logger.LoggedEntries.Single();
            await Assert.That(loggedEntry.LogLevel)
                        .IsEqualTo(LogLevel.Information);
            await Assert.That(loggedEntry.OriginalFormat)
                        .IsEqualTo("Tested structured log message {String} {Int}");
        }
    }

    [Test]
    public async Task Log_WithBasicTextLog_EntryHasNullOriginalFormat()
    {
        // Arrange
        var logger = new InMemoryLogger<StructuredLogTests>();

        // Act
        logger.LogInformation("Test Log");

        // Assert
        await Assert.That(logger.LoggedEntries)
                    .HasSingleItem();
        using (Assert.Multiple())
        {
            var loggedEntry = logger.LoggedEntries.Single();
            await Assert.That(loggedEntry.LogLevel)
                        .IsEqualTo(LogLevel.Information);
            await Assert.That(loggedEntry.OriginalFormat)
                        .IsEqualTo("Test Log");
        }
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
        using (Assert.Multiple())
        {
            var loggedEntry = logger.LoggedEntries.Single();
            await Assert.That(loggedEntry.LogLevel)
                        .IsEqualTo(LogLevel.Information);
            await Assert.That(loggedEntry.Properties)
                        .Contains(new KeyValuePair<string, object>("String", stringValue))
                        .And
                        .Contains(new KeyValuePair<string, object>("Int", intValue));
        }
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
        using (Assert.Multiple())
        {
            var loggedEntry = logger.LoggedEntries.Single();
            await Assert.That(loggedEntry.LogLevel)
                        .IsEqualTo(LogLevel.Information);
            await Assert.That(loggedEntry.Properties)
                        .Contains(p => p.Key == "@DecomposableObject" && p.Value.Equals(decomposableObject));
        }
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
        using (Assert.Multiple())
        {
            var loggedEntry = logger.LoggedEntries.Single();
            await Assert.That(loggedEntry.LogLevel)
                        .IsEqualTo(LogLevel.Information);
            await Assert.That(loggedEntry.Message)
                        .IsEqualTo("Tested structured log message 2000");
            await Assert.That(loggedEntry.Properties)
                        .Contains(new KeyValuePair<string, object>("DateTime", dateTime));
        }
    }

    public record DecomposableObject(string String, int Int);
}