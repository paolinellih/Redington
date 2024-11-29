using CalculatorAPI.Business.Services.Process.Calculator;
using CalculatorAPI.Data.Interfaces.Factories;
using CalculatorAPI.Data.Interfaces.Services;
using CalculatorAPI.Data.Responses;
using CalculatorAPI.Data.Responses.Calculator;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CalculatorAPI.UnitTests.Business.Services.Process.Calculator;

public class GetProbabilityTypeServiceTests
{
    private readonly Mock<ILogger<GetProbabilityTypeService>> _loggerMock = new();
    private readonly Mock<IProbabilityCalculatorFactory> _probabilityCalculatorFactoryMock = new();
    private GetProbabilityTypeService CreateInstance()
    {
        return new GetProbabilityTypeService(_loggerMock.Object, _probabilityCalculatorFactoryMock.Object);
    }

    [Fact]
    public async Task NoContentFoundException_ErrorProcessingRequest_DefaultErrorMessage()
    {
        // Arrange
        _probabilityCalculatorFactoryMock.Setup(r => r.GetAllCalculators()).Returns((IEnumerable<IProbabilityCalculator>)null);
        
        // Act
        var service = CreateInstance();
        var result = await service.ProcessAsync(null);

        // Assert
        Assert.NotNull(result);
        var errorResponse = result.Response as ValidationErrorResponse;
        Assert.NotNull(errorResponse);
        Assert.Contains("Request", errorResponse.ValidationErrors);
        Assert.Contains("Error processing request.", errorResponse.ValidationErrors["Request"]);
    }
    
    [Fact]
    public async Task GetProbabilityTypes_ShouldReturnAllProbabilityTypes()
    {
        // Arrange
        _probabilityCalculatorFactoryMock.Setup(r => r.GetAllCalculators()).Returns(new List<IProbabilityCalculator>
        {
            new CombinedWithCalculator(),
            new EitherCalculator(),
        });
        
        // Act
        var service = CreateInstance();
        var result = await service.ProcessAsync(null);

        // Assert
        Assert.NotNull(result);
        var response = Assert.IsType<GetProbabilityTypeResponse>(result.Response);
        Assert.True(response.Successful);
        Assert.Equal("Combined With", response.Result.FirstOrDefault()!.Value);
    }
}
