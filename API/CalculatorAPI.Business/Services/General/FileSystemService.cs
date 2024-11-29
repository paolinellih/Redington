using CalculatorAPI.Data.Interfaces.Services;

namespace CalculatorAPI.Business.Services.General;

public class FileSystemService : IFileSystemService
{
    public Task AppendAllTextAsync(string path, string content)
    {
        return File.AppendAllTextAsync(path, content);
    }
}