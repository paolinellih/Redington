using CalculatorAPI.Business.Services.Process.Calculator;
using CalculatorAPI.Data.Interfaces.Factories;
using CalculatorAPI.Data.Interfaces.Services;

namespace CalculatorAPI.Business.Factories.Services;

public class ProbabilityCalculatorFactory : IProbabilityCalculatorFactory
{
    private readonly Dictionary<string, IProbabilityCalculator> _calculators;

    public ProbabilityCalculatorFactory()
    {
        // Dynamically register calculators
        _calculators = new Dictionary<string, IProbabilityCalculator>
        {
            { "Combined With", new CombinedWithCalculator() },
            { "Either", new EitherCalculator() }
        };
    }

    public IProbabilityCalculator GetCalculator(string name)
    {
        if (_calculators.TryGetValue(name, out var calculator))
        {
            return calculator;
        }

        throw new InvalidOperationException($"Calculator not found for name: {name}");
    }

    public IEnumerable<IProbabilityCalculator> GetAllCalculators()
    {
        return _calculators.Values;
    }
}