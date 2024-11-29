using FluentValidation;

namespace CalculatorAPI.Business.Validators;

public abstract class BaseValidator<T> : AbstractValidator<T>
{
    protected BaseValidator()
    {
        // Common validation logic can be placed here if needed
    }
}