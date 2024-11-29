namespace CalculatorAPI.Data.Interfaces.Services;

public interface IFileSystemService
{
    Task AppendAllTextAsync(string path, string content);
}