namespace CalculatorAPI.Data.Interfaces.Services;

public interface IProbabilityCalculator
{
    string Name { get; } // A user-friendly name or identifier for the type
    decimal? Calculate(decimal a, decimal b);
}
