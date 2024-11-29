namespace CalculatorAPI.Data.Interfaces.Services;

public interface ILogFileWriter
{
    Task WriteLogAsync(string message);
}