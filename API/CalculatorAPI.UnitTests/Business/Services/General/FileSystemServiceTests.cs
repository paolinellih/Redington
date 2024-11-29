using CalculatorAPI.Business.Services.General;
using CalculatorAPI.Data.Interfaces.Services;
using Xunit;

namespace CalculatorAPI.UnitTests.Business.Services.General;

    public class FileSystemServiceTests
    {
        private IFileSystemService CreateInstance()
        {
            return new FileSystemService(); // Direct instantiation as no constructor dependencies
        }
        
        [Fact]
        public async Task AppendAllTextAsync_AppendsContentToFile()
        {
            // Arrange
            var filePath = "test-log.txt";
            var content = "Test log content";

            // Act
            var fileSystemService = CreateInstance();
            await fileSystemService.AppendAllTextAsync(filePath, content);

            // Assert
            Assert.True(File.Exists(filePath));

            // Cleanup: Delete the file after the test.
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }