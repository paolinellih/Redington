using CalculatorAPI.Business.Validators.Calculator;
using CalculatorAPI.Data.Enums;
using CalculatorAPI.Data.Requests.Calculator;
using Xunit;

namespace CalculatorAPI.UnitTests.Business.Validators.Calculator
{
    public class CalculateProbabilityRequestValidatorTests
    {
        private CalculateProbabilityRequest Request { get; set; } = new()
        {
            A = (decimal?)0.5,
            B = (decimal?)0.7,
            Type = "Either"
        };

        private CalculateProbabilityRequestValidator CreateInstance()
        {
            return new();
        }

        [Fact]
        public async Task EmptyRequest()
        {
            var request = new CalculateProbabilityRequest();
            var validator = CreateInstance();
            var validatorResults = await validator.ValidateAsync(request);

            Assert.False(validatorResults.IsValid);
            Assert.Equal(3, validatorResults.Errors.Count); // All fields should fail
            Assert.Contains(validatorResults.Errors, e => e.PropertyName == "A" && e.ErrorMessage == "Probability A is a required field.");
            Assert.Contains(validatorResults.Errors, e => e.PropertyName == "B" && e.ErrorMessage == "Probability B is a required field.");
            Assert.Contains(validatorResults.Errors, e => e.PropertyName == "Type" && e.ErrorMessage == "Type is a required field.");
        }

        [Fact]
        public async Task InvalidAValue()
        {
            Request.A = (decimal?)1.5; // Invalid value for A
            var validator = CreateInstance();
            var validatorResults = await validator.ValidateAsync(Request);

            Assert.False(validatorResults.IsValid);
            Assert.Single(validatorResults.Errors);
            Assert.Contains(validatorResults.Errors, e => e.PropertyName == "A" && e.ErrorMessage == "Probability A must be between 0 and 1.");
        }

        [Fact]
        public async Task InvalidBValue()
        {
            Request.B = (decimal?)-0.3; // Invalid value for B
            var validator = CreateInstance();
            var validatorResults = await validator.ValidateAsync(Request);

            Assert.False(validatorResults.IsValid);
            Assert.Single(validatorResults.Errors);
            Assert.Contains(validatorResults.Errors, e => e.PropertyName == "B" && e.ErrorMessage == "Probability B must be between 0 and 1.");
        }

        [Fact]
        public async Task InvalidTypeValue()
        {
            Request.Type = "Not a type"; // Invalid value
            var validator = CreateInstance();
            var validatorResults = await validator.ValidateAsync(Request);

            Assert.False(validatorResults.IsValid);
            Assert.Single(validatorResults.Errors);
            Assert.Contains(validatorResults.Errors, e => e.PropertyName == "Type" && e.ErrorMessage.StartsWith("Type must be one of the following"));
        }

        [Fact]
        public async Task SuccessfulRequest()
        {
            var validator = CreateInstance();
            var validatorResults = await validator.ValidateAsync(Request);

            Assert.True(validatorResults.IsValid);
            Assert.Empty(validatorResults.Errors);
        }
    }
}
