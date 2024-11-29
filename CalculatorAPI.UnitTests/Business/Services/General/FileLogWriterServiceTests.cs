using CalculatorAPI.Business.Services.General;
using Xunit;

namespace CalculatorAPI.UnitTests.Business.Services.General;

public class FileLogWriterServiceTests
{
    private readonly FileLogWriterService _service;

    public FileLogWriterServiceTests()
    {
        _service = new FileLogWriterService();
    }

    [Fact]
    public async Task WriteLogAsync_CreatesLogFileIfNotExists()
    {
        // Arrange
        var message = "Test log message";
        var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        var logFilePath = Path.Combine(logDirectory, $"calculator-api-service-log-{DateTime.Now:yyyy-MM-dd}.txt");

        if (File.Exists(logFilePath))
        {
            File.Delete(logFilePath); // Ensure a clean start
        }

        // Act
        await _service.WriteLogAsync(message);

        // Assert
        Assert.True(File.Exists(logFilePath));
        var fileContent = await File.ReadAllTextAsync(logFilePath);
        Assert.Contains(message, fileContent);
    }

    [Fact]
    public async Task WriteLogAsync_AppendsToExistingLogFile()
    {
        // Arrange
        var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        var logFilePath = Path.Combine(logDirectory, $"calculator-api-service-log-{DateTime.Now:yyyy-MM-dd}.txt");
        var initialMessage = "Initial log message";
        var appendedMessage = "Appended log message";

        Directory.CreateDirectory(logDirectory); // Ensure directory exists
        await File.WriteAllTextAsync(logFilePath, initialMessage + Environment.NewLine); // Seed file with initial content

        // Act
        await _service.WriteLogAsync(appendedMessage);

        // Assert
        var fileContent = await File.ReadAllTextAsync(logFilePath);
        Assert.Contains(initialMessage, fileContent);
        Assert.Contains(appendedMessage, fileContent);
    }

    [Fact]
    public async Task WriteLogAsync_HandlesEmptyMessage()
    {
        // Arrange
        var message = string.Empty;
        var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        var logFilePath = Path.Combine(logDirectory, $"calculator-api-service-log-{DateTime.Now:yyyy-MM-dd}.txt");

        // Act
        await _service.WriteLogAsync(message);

        // Assert
        Assert.True(File.Exists(logFilePath));
        var fileContent = await File.ReadAllTextAsync(logFilePath);
        Assert.Contains(Environment.NewLine, fileContent); // New line should still be present
    }
}