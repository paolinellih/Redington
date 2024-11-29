using CalculatorAPI.Data.Enums;
using CalculatorAPI.Data.Interfaces.Repositories;
using CalculatorAPI.Data.Responses.Calculator;

namespace CalculatorAPI.Business.Repositories;
public class CalculateProbabilityRepository : ICalculateProbabilityRepository
{
    public IEnumerable<ProbabilityTypeDto> GetProbabilityTypes()
    {
        return Enum.GetValues(typeof(ProbabilityType))
            .Cast<ProbabilityType>()
            .Select(type => new ProbabilityTypeDto
            {
                Value = (int)type,
                Name = type.ToString()
            });
    }
}