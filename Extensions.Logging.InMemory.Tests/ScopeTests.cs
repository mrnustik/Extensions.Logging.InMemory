using Microsoft.Extensions.Logging;

namespace Extensions.Logging.InMemory.Tests;

public class ScopeTests
{
    [Test]
    public async Task Log_WithOpenDictionaryScope_ShouldHaveScopeProperties()
    {
        // Arrange
        var logger = new InMemoryLogger<ScopeTests>();

        // Act
        using (logger.BeginScope(new Dictionary<string, object>
               {
                   { "ScopeProperty", "ScopeValue" }
               }))
        {
            logger.LogInformation("Something {LogProperty}", "LogValue");
        }

        // Assert
        await Assert.That(logger.LoggedEntries)
                    .IsPartiallyEquivalentTo(new object[]
                    {
                        new
                        {
                            Message = "Something LogValue",
                            Properties = new Dictionary<string, object>
                            {
                                { "{OriginalFormat}", "Something {LogProperty}" },
                                { "LogProperty", "LogValue" },
                                { "ScopeProperty", "ScopeValue" }
                            }
                        }
                    });
    }

    [Test]
    public async Task Log_WithOpenMessageScope_ShouldHaveScopeProperties()
    {
        // Arrange
        var logger = new InMemoryLogger<ScopeTests>();

        // Act
        using (logger.BeginScope("Something {ScopeProperty}", "ScopeValue"))
        {
            logger.LogInformation("Something {LogProperty}", "LogValue");
        }

        // Assert
        await Assert.That(logger.LoggedEntries)
                    .IsPartiallyEquivalentTo(new object[]
                    {
                        new
                        {
                            Message = "Something LogValue",
                            Properties = new Dictionary<string, object>
                            {
                                { "{OriginalFormat}", "Something {LogProperty}" },
                                { "LogProperty", "LogValue" },
                                { "ScopeProperty", "ScopeValue" }
                            }
                        }
                    });
    }
}