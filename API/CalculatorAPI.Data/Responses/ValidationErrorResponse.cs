namespace CalculatorAPI.Data.Responses
{
    public class ValidationErrorResponse : BaseResponse
    {
        public Dictionary<string, List<string>> ValidationErrors { get; set; }
    }

}