namespace CalculatorAPI.Data.Interfaces.Services;

public interface IFileLogWriterService
{
    Task WriteLogAsync(string message);
}