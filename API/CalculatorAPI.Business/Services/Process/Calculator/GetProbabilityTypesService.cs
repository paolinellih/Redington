using CalculatorAPI.Data.Exceptions;
using CalculatorAPI.Data.Interfaces;
using CalculatorAPI.Data.Interfaces.Factories;
using CalculatorAPI.Data.Responses;
using CalculatorAPI.Data.Responses.Calculator;
using Microsoft.Extensions.Logging;

namespace CalculatorAPI.Business.Services.Process.Calculator;

public class GetProbabilityTypeService(
    ILogger<GetProbabilityTypeService> logger,
    IProbabilityCalculatorFactory calculatorFactory)
    : HttpBaseProcessService<GetProbabilityTypeService>(logger)
{
    protected override async Task<HttpProcessResult> InternalProcess(IRequest baseRequest = null)
    {
        // Get the probability type list
        var result  = calculatorFactory.GetAllCalculators();
        
        if (result == null || result.ToArray().Count() == 1)
        {
            // Throw no content instead of not found.
            throw new NoContentFoundException();
        }
        
        // Build the response and status code
        return new HttpProcessResult
        {
            Response = new GetProbabilityTypeResponse
            {
                Result = result.Select(r => new ProbabilityTypeDto
                {
                    Value = r.Name,
                    Name = r.Name
                }).ToList(),
                Successful = true
            }
        };
    }
}