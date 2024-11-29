using CalculatorAPI.Data.Interfaces.Services;

namespace CalculatorAPI.Business.Services.General;

public class FileLogWriterService : ILogFileWriter
{
    public async Task WriteLogAsync(string message)
    {
        // Create new file if day changes
        try
        {
            var logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", $"calculator-api-service-log-{DateTime.Now:yyyy-MM-dd}.txt");
            await File.AppendAllTextAsync(logFilePath, message + Environment.NewLine);
        }
        catch (Exception)
        {
            // ignored
        }
    }
}
