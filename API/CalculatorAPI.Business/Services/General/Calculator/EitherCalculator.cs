using CalculatorAPI.Data.Interfaces.Services;

namespace CalculatorAPI.Business.Services.Process.Calculator;

public class EitherCalculator : IProbabilityCalculator
{
    public string Name => "Either"; // Friendly name

    public decimal? Calculate(decimal a, decimal b)
    {
        return a + b - (a * b);
    }
}
