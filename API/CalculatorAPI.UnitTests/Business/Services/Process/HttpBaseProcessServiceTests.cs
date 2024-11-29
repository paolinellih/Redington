using CalculatorAPI.Business.Services.Process;
using CalculatorAPI.Data.Interfaces;
using CalculatorAPI.Data.Responses;
using CalculatorAPI.Data.Responses.Calculator;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CalculatorAPI.UnitTests.Business.Services.Process
{
    public class HttpBaseProcessServiceTests
    {
        private readonly Mock<ILogger<TestableHttpBaseProcessService>> _loggerMock;
        private readonly Mock<IValidator> _validatorMock;

        public HttpBaseProcessServiceTests()
        {
            _loggerMock = new Mock<ILogger<TestableHttpBaseProcessService>>();
            _validatorMock = new Mock<IValidator>();
        }

        [Fact]
        public async Task ProcessAsync_ShouldReturnValidResult_WhenRequestIsValid()
        {
            // Arrange
            var requestMock = new Mock<IRequest>();
            var expectedResponse = new HttpProcessResult
            {
                Response = new CalculateProbabilityResponse
                {
                    Result = 0.5m,
                    Successful = true
                }
            };

            var service = new TestableHttpBaseProcessService(
                _loggerMock.Object,
                _validatorMock.Object,
                req => Task.FromResult(expectedResponse)); // Injecting behavior

            // Act
            var result = await service.ProcessAsync(requestMock.Object);

            // Assert
            Assert.NotNull(result);
            var response = result.Response as CalculateProbabilityResponse;
            Assert.NotNull(response);
            Assert.Equal(0.5m, response.Result);
            Assert.True(response.Successful);
        }

        [Fact]
        public async Task ProcessAsync_ShouldReturnValidationError_WhenValidationFails()
        {
            // Arrange
            var requestMock = new Mock<IRequest>();
            var validationErrors = new List<ValidationFailure>
            {
                new ValidationFailure("Field", "Error message")
            };
            var validationResult = new ValidationResult(validationErrors);
            _validatorMock.Setup(v => v.Validate(It.IsAny<ValidationContext<IRequest>>()))
                          .Returns(validationResult);

            var service = new TestableHttpBaseProcessService(
                _loggerMock.Object,
                _validatorMock.Object);

            // Act
            var result = await service.ProcessAsync(requestMock.Object);

            // Assert
            Assert.NotNull(result);
            var errorResponse = result.Response as ValidationErrorResponse;
            Assert.NotNull(errorResponse);
            Assert.Contains("Request", errorResponse.ValidationErrors);
            Assert.Contains("Error message", errorResponse.ValidationErrors["Request"]);
        }
        
        [Fact]
        public async Task ProcessAsync_ShouldThrowException_WhenExceptionOccurs()
        {
            // Arrange
            var requestMock = new Mock<IRequest>();
            var service = new TestableHttpBaseProcessService(
                _loggerMock.Object,
                _validatorMock.Object,
                req => throw new ValidationException("Unexpected error"));

            // Act
            var result = await service.ProcessAsync(requestMock.Object);

            // Assert
            Assert.NotNull(result);
            var errorResponse = result.Response as ValidationErrorResponse;
            Assert.NotNull(errorResponse);
            Assert.Contains("Request", errorResponse.ValidationErrors);
            Assert.Contains("Error processing request.", errorResponse.ValidationErrors["Request"]);
        }

        [Fact]
        public async Task ProcessAsync_ShouldReturnErrorResponse_WhenExceptionOccurs()
        {
            // Arrange
            var requestMock = new Mock<IRequest>();
            var service = new TestableHttpBaseProcessService(
                _loggerMock.Object,
                _validatorMock.Object,
                req => throw new Exception("Unexpected error"));

            // Act
            var result = await service.ProcessAsync(requestMock.Object);

            // Assert
            Assert.NotNull(result);
            var errorResponse = result.Response as ValidationErrorResponse;
            Assert.NotNull(errorResponse);
            Assert.Contains("Request", errorResponse.ValidationErrors);
            Assert.Contains("Error processing request.", errorResponse.ValidationErrors["Request"]);
        }

        [Fact]
        public async Task ProcessAndReturnJsonAsync_ShouldReturnJsonString()
        {
            // Arrange
            var requestMock = new Mock<IRequest>();
            var expectedResponse = new HttpProcessResult
            {
                Response = new CalculateProbabilityResponse
                {
                    Result = 0.8m,
                    Successful = true
                }
            };

            var service = new TestableHttpBaseProcessService(
                _loggerMock.Object,
                _validatorMock.Object,
                req => Task.FromResult(expectedResponse));

            // Act
            var jsonResult = await service.ProcessAndReturnJsonAsync(requestMock.Object);

            // Assert
            Assert.NotNull(jsonResult);
            Assert.Contains("\"Result\":0.8", jsonResult);
            Assert.Contains("\"Successful\":true", jsonResult);
        }

        [Fact]
        public async Task ProcessAndReturnJsonAsync_ShouldHandleNullRequest()
        {
            // Arrange
            var service = new TestableHttpBaseProcessService(
                _loggerMock.Object,
                _validatorMock.Object,
                req => Task.FromResult(new HttpProcessResult
                {
                    Response = new ValidationErrorResponse
                    {
                        ValidationErrors = new Dictionary<string, List<string>>
                        {
                            { "Request", new List<string> { "Null request error" } }
                        }
                    }
                }));

            // Act
            var jsonResult = await service.ProcessAndReturnJsonAsync();

            // Assert
            Assert.NotNull(jsonResult);
            Assert.Contains("\"Null request error\"", jsonResult);
        }
    }
    
    // Testable version of HttpBaseProcessService
    public class TestableHttpBaseProcessService : HttpBaseProcessService<TestableHttpBaseProcessService>
    {
        private readonly Func<IRequest, Task<HttpProcessResult>> _processImplementation;

        public TestableHttpBaseProcessService(
            ILogger<TestableHttpBaseProcessService> logger,
            IValidator validator,
            Func<IRequest, Task<HttpProcessResult>> processImplementation = null)
            : base(logger, validator)
        {
            // Allow test logic for InternalProcess to be injected
            _processImplementation = processImplementation ?? (req => Task.FromResult(new HttpProcessResult()));
        }

        // Override InternalProcess to allow test control
        protected override Task<HttpProcessResult> InternalProcess(IRequest request)
        {
            return _processImplementation(request);
        }
    }
}