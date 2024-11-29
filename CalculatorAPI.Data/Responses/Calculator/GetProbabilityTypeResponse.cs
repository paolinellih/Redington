namespace CalculatorAPI.Data.Responses.Calculator;

public class GetProbabilityTypeResponse : BaseResponse
{
    public List<ProbabilityTypeDto> Result { get; set; }
}

public class ProbabilityTypeDto
{
    public int Value { get; set; }
    public string Name { get; set; }
}