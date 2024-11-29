using CalculatorAPI.Business.Factories.Services;
using CalculatorAPI.Business.Services.Process.Calculator;
using CalculatorAPI.Data.Interfaces.Services;
using Xunit;

namespace CalculatorAPI.UnitTests.Business.Factories;

public class ProbabilityCalculatorFactoryTests
{
    private readonly ProbabilityCalculatorFactory _factory = new();

    [Fact]
    public void GetCalculator_ValidName_ReturnsCorrectCalculator()
    {
        // Arrange
        var validCalculatorName = "Combined With";

        // Act
        var calculator = _factory.GetCalculator(validCalculatorName);

        // Assert
        Assert.NotNull(calculator);
        Assert.IsType<CombinedWithCalculator>(calculator); // Verify that the correct calculator is returned
    }

    [Fact]
    public void GetCalculator_InvalidName_ThrowsInvalidOperationException()
    {
        // Arrange
        var invalidCalculatorName = "NonExistentCalculator";

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _factory.GetCalculator(invalidCalculatorName));
        Assert.Equal("Calculator not found for name: NonExistentCalculator", exception.Message);
    }

    [Fact]
    public void GetAllCalculators_ReturnsAllCalculators()
    {
        // Act
        var calculators = _factory.GetAllCalculators();

        // Assert
        Assert.NotNull(calculators);
        var calculatorsList = new List<IProbabilityCalculator>(calculators);
        Assert.Equal(2, calculatorsList.Count); // Ensure there are 2 calculators in the dictionary
        Assert.Contains(calculatorsList, calc => calc is CombinedWithCalculator);
        Assert.Contains(calculatorsList, calc => calc is EitherCalculator);
    }
}