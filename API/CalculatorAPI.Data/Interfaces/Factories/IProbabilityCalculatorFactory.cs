using CalculatorAPI.Data.Interfaces.Services;

namespace CalculatorAPI.Data.Interfaces.Factories;

public interface IProbabilityCalculatorFactory
{
    IProbabilityCalculator GetCalculator(string name);
    IEnumerable<IProbabilityCalculator> GetAllCalculators(); // New method to list all calculators
}