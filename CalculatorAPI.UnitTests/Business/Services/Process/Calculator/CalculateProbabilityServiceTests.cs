using CalculatorAPI.Business.Services.Process.Calculator;
using CalculatorAPI.Business.Validators.Calculator;
using CalculatorAPI.Data.Enums;
using CalculatorAPI.Data.Interfaces.Services;
using CalculatorAPI.Data.Requests.Calculator;
using CalculatorAPI.Data.Responses;
using CalculatorAPI.Data.Responses.Calculator;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CalculatorAPI.UnitTests.Business.Services.Process.Calculator;

public class CalculateProbabilityServiceTests
{
    private readonly Mock<ILogger<CalculateProbabilityService>> _loggerMock = new();
    private readonly Mock<ILogFileWriter> _logFileWriterMock = new();
    private CalculateProbabilityService CreateInstance()
    {
        return new CalculateProbabilityService(_loggerMock.Object, _logFileWriterMock.Object);
    }

    [Fact]
    public async Task CombinedWith_ValidRequest_ReturnsExpectedResult()
    {
        // Arrange
        var request = new CalculateProbabilityRequest
        {
            A = 0.5m,
            B = 0.4m,
            Type = ProbabilityType.CombinedWith
        };

        // Act
        var service = CreateInstance();
        var result = await service.ProcessAsync(request);

        // Assert
        Assert.NotNull(result);
        var response = Assert.IsType<CalculateProbabilityResponse>(result.Response);
        Assert.True(response.Successful);
        Assert.Equal(0.2m, response.Result);
    }

    [Fact]
    public async Task Either_ValidRequest_ReturnsExpectedResult()
    {
        // Arrange
        var request = new CalculateProbabilityRequest
        {
            A = 0.5m,
            B = 0.4m,
            Type = ProbabilityType.Either
        };

        // Act
        var service = CreateInstance();
        var result = await service.ProcessAsync(request);

        // Assert
        Assert.NotNull(result);
        var response = Assert.IsType<CalculateProbabilityResponse>(result.Response);
        Assert.True(response.Successful);
        Assert.Equal(0.7m, response.Result);
    }

    [Fact]
    public async Task CombinedWith_ValidRequest_ButNullResult_ThrowsValidationException()
    {
        // Arrange
        var request = new CalculateProbabilityRequest
        {
            A = 0.5m,
            B = 0.4m,
            Type = ProbabilityType.SelectAnOption
        };

        // Act
        var service = CreateInstance();
        var result = await service.ProcessAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Response is ValidationErrorResponse);

        var validationErrorResponse = (ValidationErrorResponse)result.Response;
        Assert.Single(validationErrorResponse.ValidationErrors);
        Assert.Equal("Invalid calculation type.", validationErrorResponse.ValidationErrors["Request"].FirstOrDefault());
    }

    [Fact]
    public async Task LogsCorrectMessage_ForCombinedWithCalculation()
    {
        // Arrange
        var request = new CalculateProbabilityRequest
        {
            A = 0.5m,
            B = 0.4m,
            Type = ProbabilityType.CombinedWith
        };

        var expectedLogMessage = $"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}, " +
                                 $"Calculation Type: {request.Type}, " +
                                 $"Inputs: A={request.A}, B={request.B}, " +
                                 $"Result: 0.2";

        // Act
        var service = CreateInstance();
        await service.ProcessAsync(request);

        // Assert
        _logFileWriterMock.Verify(
            writer => writer.WriteLogAsync(It.Is<string>(message => message.Contains($"Calculation Type: {request.Type}") &&
                                                                    message.Contains($"Inputs: A={request.A}, B={request.B}") &&
                                                                    message.Contains("Result: 0.2"))),
            Times.Once);
    }
}
