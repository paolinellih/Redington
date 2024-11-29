using CalculatorAPI.Data.Responses;

namespace CalculatorAPI.Data.Interfaces;

public interface IHttpProcessService
{
    Task<HttpProcessResult> ProcessAsync(IRequest request);
    Task<string> ProcessAndReturnJsonAsync(object request);
    Task<string> ProcessAndReturnJsonAsync();
}