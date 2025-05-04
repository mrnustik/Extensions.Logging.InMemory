# Extensions.Logging.InMemory

[![publish](https://github.com/mrnustik/Extensions.Logging.InMemory/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/mrnustik/Extensions.Logging.InMemory/actions/workflows/dotnet.yml)
[![NuGet Version](https://img.shields.io/nuget/v/Extensions.Logging.InMemory)](https://www.nuget.org/packages/Extensions.Logging.InMemory/)
![GitHub License](https://img.shields.io/github/license/mrnustik/Extensions.Logging.InMemory)

An in-memory logged implementation for Microsoft.Extensions.Logging logger designed for use in tests.

## Supported features

 - Structured logging
 - Scope properties
 - Source-generated logs

## Sample usage:

```csharp
public class ClassTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        var logger = new InMemoryLogger<Class>();
        var sut = new Class(logger);
        
        // Act
        sut.DoSomething(42);

        // Assert
        var loggedEntry = logger.LoggedEntries.Single();
        Assert.Equal("DoSomething called 42", loggedEntry.Message);
        Assert.Equal("DoSomething called {Value}", loggedEntry.OriginalFormat);
        Assert.Contains(loggedEntry.Properties, item => item.Key == "Value" && item.Value == 42);
    }
}

public class Class 
{
    private readonly ILogger<Class> _logger;
    
    public Class(ILogger<Class> logger)
    {
        _logger = logger;
    }
    
    public void DoSomething(int value) 
    {
        _logger.LogInformation("DoSomething called {Value}", value);
    }
}
```