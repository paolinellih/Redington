using CalculatorAPI.Business.Services.Process.Calculator;
using CalculatorAPI.Business.Validators.Calculator;
using CalculatorAPI.Data.Enums;
using CalculatorAPI.Data.Exceptions;
using CalculatorAPI.Data.Interfaces.Repositories;
using CalculatorAPI.Data.Interfaces.Services;
using CalculatorAPI.Data.Requests.Calculator;
using CalculatorAPI.Data.Responses;
using CalculatorAPI.Data.Responses.Calculator;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CalculatorAPI.UnitTests.Business.Services.Process.Calculator;

public class GetProbabilityTypeServiceTests
{
    private readonly Mock<ILogger<GetProbabilityTypeService>> _loggerMock = new();
    private readonly Mock<ICalculateProbabilityRepository> _calculateProbabilityRepositoryMock = new();
    private GetProbabilityTypeService CreateInstance()
    {
        return new GetProbabilityTypeService(_loggerMock.Object, _calculateProbabilityRepositoryMock.Object);
    }

    [Fact]
    public async Task NoContentFoundException_ErrorProcessingRequest_DefaultErrorMessage()
    {
        // Arrange
        _calculateProbabilityRepositoryMock.Setup(r => r.GetProbabilityTypes()).Returns((IEnumerable<ProbabilityTypeDto>)null);
        
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
        _calculateProbabilityRepositoryMock.Setup(r => r.GetProbabilityTypes()).Returns(new List<ProbabilityTypeDto>
        {
            new ProbabilityTypeDto
            {
                Value = 1,
                Name = "Type 1"
            }
        });
        
        // Act
        var service = CreateInstance();
        var result = await service.ProcessAsync(null);

        // Assert
        Assert.NotNull(result);
        var response = Assert.IsType<GetProbabilityTypeResponse>(result.Response);
        Assert.True(response.Successful);
        Assert.Equal(1, response.Result.FirstOrDefault()!.Value);
    }
}
