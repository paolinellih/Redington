using CalculatorAPI.Business.Services.General;
using CalculatorAPI.Data.Interfaces.Services;
using Moq;
using Xunit;

namespace CalculatorAPI.UnitTests.Business.Services.General;

public class FileLogWriterServiceTests 
{
    private readonly Mock<IFileSystemService> _fileSystemServiceMock = new();

    // CreateInstance method to instantiate FileLogWriterServiceService with mocks
    private FileLogWriterService CreateInstance()
    {
        return new FileLogWriterService(_fileSystemServiceMock.Object);
    }

    // This test verifies that WriteLogAsync behaves correctly when no exception occurs
    [Fact]
    public async Task WriteLogAsync_WritesLogSuccessfully()
    {
        // Arrange
        var message = "Test log message";
        var logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", $"calculator-api-service-log-{DateTime.Now:yyyy-MM-dd}.txt");

        // Mocking the IFileSystemService to behave normally
        _fileSystemServiceMock
            .Setup(f => f.AppendAllTextAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask); // Simulating a successful log write

        // Act
        var service = CreateInstance();
        await service.WriteLogAsync(message);

        // Assert
        // Ensure that the AppendAllTextAsync method was called with the correct parameters
        _fileSystemServiceMock.Verify(f => f.AppendAllTextAsync(logFilePath, message + Environment.NewLine), Times.Once);
    }
}