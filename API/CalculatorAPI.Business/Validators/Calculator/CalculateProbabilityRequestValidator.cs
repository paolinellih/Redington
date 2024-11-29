using CalculatorAPI.Business.Helper;
using CalculatorAPI.Data.Enums;
using CalculatorAPI.Data.Requests.Calculator;
using FluentValidation;

namespace CalculatorAPI.Business.Validators.Calculator;

public class CalculateProbabilityRequestValidator : BaseValidator<CalculateProbabilityRequest>
{
    public CalculateProbabilityRequestValidator()
    {
        RuleFor(x => x.A)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Probability A is a required field.")
            .InclusiveBetween(0, 1).WithMessage("Probability A must be between 0 and 1.");
        
        RuleFor(x => x.B)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Probability B is a required field.")
            .InclusiveBetween(0, 1).WithMessage("Probability B must be between 0 and 1.");
        
        RuleFor(x => x.Type)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Type is a required field.")
            .Must(BeAValidProbabilityType).WithMessage($"Type must be one of the following: {string.Join(", ", EnumHelper.GetEnumValues<ProbabilityType>().Split(','))}.");
    }
    
    // Custom rule to check if the Type is one of the valid enum values
    private bool BeAValidProbabilityType(string type)
    {
        if (type == null)
            return false;

        var validValues = EnumHelper.GetEnumValues<ProbabilityType>().Split(',').Select(v => v.Trim()).ToList();
        var typeName = type;
        return validValues.Contains(typeName);
    }
}