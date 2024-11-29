using CalculatorAPI.Data.Interfaces.Services;

namespace CalculatorAPI.Business.Services.Process.Calculator;

public class CombinedWithCalculator : IProbabilityCalculator
{
    public string Name => "Combined With"; // Friendly name

    public decimal? Calculate(decimal a, decimal b)
    {
        return a * b;
    }
}