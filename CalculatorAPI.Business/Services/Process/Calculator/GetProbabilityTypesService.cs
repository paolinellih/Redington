using CalculatorAPI.Data.Enums;
using CalculatorAPI.Data.Exceptions;
using CalculatorAPI.Data.Interfaces;
using CalculatorAPI.Data.Interfaces.Repositories;
using CalculatorAPI.Data.Responses;
using CalculatorAPI.Data.Responses.Calculator;
using Microsoft.Extensions.Logging;

namespace CalculatorAPI.Business.Services.Process.Calculator;

public class GetProbabilityTypeService(
    ILogger<GetProbabilityTypeService> logger,
    ICalculateProbabilityRepository calculateProbabilityRepository)
    : HttpBaseProcessService<GetProbabilityTypeService>(logger)
{
    protected override async Task<HttpProcessResult> InternalProcess(IRequest baseRequest = null)
    {
        // Get the probability type list
        var result = calculateProbabilityRepository.GetProbabilityTypes();

        if (result == null)
        {
            // Throw no content instead of not found.
            throw new NoContentFoundException();
        }

        // Exclude SelectAnOption as it will be managed by the fron end
        result = result.Where(r => r.Value != (int)ProbabilityType.SelectAnOption).ToList();
        
        // Build the response and status code
        return new HttpProcessResult
        {
            Response = new GetProbabilityTypeResponse
            {
                Result = result.Select(r => new ProbabilityTypeDto
                {
                    Value = r.Value,
                    Name = r.Name
                }).ToList(),
                Successful = true
            }
        };
    }
}