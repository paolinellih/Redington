using CalculatorAPI.Business.Validators.Calculator;
using CalculatorAPI.Data.Enums;
using CalculatorAPI.Data.Interfaces;
using CalculatorAPI.Data.Interfaces.Services;
using CalculatorAPI.Data.Requests.Calculator;
using CalculatorAPI.Data.Responses;
using CalculatorAPI.Data.Responses.Calculator;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CalculatorAPI.Business.Services.Process.Calculator;

public class CalculateProbabilityService(
    ILogger<CalculateProbabilityService> logger,
    ILogFileWriter logFileWriter)
    : HttpBaseProcessService<CalculateProbabilityService>(logger, new CalculateProbabilityRequestValidator())
{
    protected override async Task<HttpProcessResult> InternalProcess(IRequest baseRequest = null)
    {
        var request = baseRequest as CalculateProbabilityRequest;
        
        decimal? result = null;
        
        // Calculate probability
        switch (request!.Type)
        {
            case ProbabilityType.CombinedWith:
                result = request.A * request.B;
                break;

            case ProbabilityType.Either:
                result = request.A + request.B - (request.A * request.B);
                break;

            default:
                // Add custom validation error
                ValidationResults ??= new Dictionary<string, List<string>>();
                ValidationResults.Add("Request", ["Invalid calculation type."]);
                throw new ValidationException("There has been a validation error.");
        }
        
        // Log the calculation details
        var logMessage = $"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}, " +
                         $"Calculation Type: {request.Type}, " +
                         $"Inputs: A={request.A}, B={request.B}, " +
                         $"Result: {result}";
        
        // Just logs it. No need to wait for return value
        _ = Task.Run(async () =>
        {
            try
            {
                await logFileWriter.WriteLogAsync(logMessage);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Failed to write log: {Message}", logMessage);
                // Optionally: Add information to a monitoring system
            }
        });

        // Build the response and status code
        return new HttpProcessResult
        {
            Response = new CalculateProbabilityResponse
            {
                Result = result ?? 0,
                Successful = true
            }
        };
    }
}