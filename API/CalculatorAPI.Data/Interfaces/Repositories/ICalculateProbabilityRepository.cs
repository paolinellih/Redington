using CalculatorAPI.Data.Enums;
using CalculatorAPI.Data.Responses.Calculator;

namespace CalculatorAPI.Data.Interfaces.Repositories;

public interface ICalculateProbabilityRepository
{
    IEnumerable<ProbabilityTypeDto> GetProbabilityTypes();
}