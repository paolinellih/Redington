using System.Diagnostics.CodeAnalysis;
using CalculatorAPI.Data.Interfaces;
using CalculatorAPI.Data.Requests.Calculator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CalculatorAPI.Business.ApiEndpoints;

[ExcludeFromCodeCoverage]
public static class CalculatorApiEndpoints
{
    private const string BaseUri = "api/Calculator";
    
    public static void AddCalculatorApiEndpoints(this WebApplication app)
    {
        const string baseUrl = $"{BaseUri}";

        app.MapPost($"{baseUrl}/calculate-probability",
            async ([FromBody] CalculateProbabilityRequest request,
                    [FromKeyedServices("CalculatorCalculateProbabilityService")] IHttpProcessService service) =>
                await service.ProcessAndReturnJsonAsync(request));

        app.MapGet($"{baseUrl}/probability-types",
            async ([FromKeyedServices("CalculatorGetProbabilityTypeService")] IHttpProcessService service) =>
            await service.ProcessAndReturnJsonAsync());
    }
}