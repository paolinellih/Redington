using CalculatorAPI.Data.Interfaces.Services;

namespace CalculatorAPI.Business.Services.General;

public class FileLogWriterServiceService(IFileSystemService fileSystemService) : IFileLogWriterService
{
    public async Task WriteLogAsync(string message)
    {
        var logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", $"calculator-api-service-log-{DateTime.Now:yyyy-MM-dd}.txt");
        await fileSystemService.AppendAllTextAsync(logFilePath, message + Environment.NewLine);
    }
}
