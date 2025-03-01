namespace Extensions.Logging.InMemory.Tests;

public class EmptyTest
{
    [Test]
    public async Task True_Should_BeTrue()
    {
        // Arrange
        var value = true;
        
        // Assert
        await Assert.That(value)
                    .IsTrue();
    }
}