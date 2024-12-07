using CalculatorAPI.Data.Interfaces;
using CalculatorAPI.Data.Responses;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ValidationException = FluentValidation.ValidationException;

namespace CalculatorAPI.Business.Services.Process
{
    // HttpBaseProcessService<TService> implements IHttpProcessService
    public abstract class HttpBaseProcessService<TService> : IHttpProcessService
    {
        protected readonly ILogger<TService> Logger;
        private readonly IValidator? _requestValidator;

        // Constructor allows request validator to be optional
        protected HttpBaseProcessService(
            ILogger<TService> logger,
            IValidator? requestValidator = null)  // Optional
        {
            Logger = logger;
            _requestValidator = requestValidator;
        }
        
        // Stores validation errors
        protected Dictionary<string, List<string>> ValidationResults { get; set; }

        // Abstract method to be overridden by services
        protected abstract Task<HttpProcessResult> InternalProcess(IRequest request);

        // Public method to validate the request and call the internal process
        public async Task<HttpProcessResult> ProcessAsync(IRequest request)
        {
            // Initialize ValidationResults dictionary if not already done
            ValidationResults ??= new Dictionary<string, List<string>>();

            // Proceed with validation if a validator is provided
            if (_requestValidator != null)
            {
                var validationResult = _requestValidator.Validate(new ValidationContext<IRequest>(request));
                if (validationResult != null && !validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        if (!ValidationResults.ContainsKey("Request"))
                        {
                            ValidationResults.Add("Request", new List<string>());
                        }
                        ValidationResults["Request"].Add(error.ErrorMessage);
                    }

                    // Return HttpProcessResult with ValidationErrorResponse as Response
                    return new HttpProcessResult
                    {
                        Response = new ValidationErrorResponse
                        {
                            ValidationErrors = ValidationResults,
                            Successful = false
                        }
                    };
                }
            }
            
            // Attempt to execute the main processing logic
            try
            {
                var result = await InternalProcess(request);
                return result;
            }
            catch (Exception ex)
            {
                if (ex is ValidationException && ValidationResults.Count > 0)
                {
                    return new HttpProcessResult
                    {
                        Response = new ValidationErrorResponse
                        {
                            ValidationErrors = ValidationResults,
                            Successful = false
                        }
                    };
                }
                
                Logger.LogError(ex, "Error processing request");
                
                // Send a default ValidationErrorResponse
                ValidationResults.Add("Request", ["Error processing request."]);
                return new HttpProcessResult
                {
                    Response = new ValidationErrorResponse
                    {
                        ValidationErrors = ValidationResults,
                        Successful = false
                    }
                };
            }
        }
        
        // ProcessAndReturnJsonAsync(object request)
        public async Task<string> ProcessAndReturnJsonAsync(object request)
        {
            var processResult = await ProcessAsync(request as IRequest);

            // Use camelCase for JSON property names
            var jsonResult = JsonConvert.SerializeObject(processResult, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });

            return jsonResult;
        }

        // ProcessAndReturnJsonAsync() - No argument
        public async Task<string> ProcessAndReturnJsonAsync()
        {
            // Call the internal process without a request object (assuming this is handled in the implementation)
            var processResult = await InternalProcess(null!);

            // Use camelCase for JSON property names
            var jsonResult = JsonConvert.SerializeObject(processResult, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });

            return jsonResult;
        }
    }
    
}
