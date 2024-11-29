using CalculatorAPI.Business.Repositories;
using CalculatorAPI.Data.Interfaces.Repositories;
using CalculatorAPI.Data.Enums;
using Xunit;

namespace CalculatorAPI.UnitTests.Business.Repositories
{
    public class CalculateProbabilityRepositoryTests
    {
        private ICalculateProbabilityRepository CreateInstance()
        {
            return new CalculateProbabilityRepository();
        }

        [Fact]
        public void GetProbabilityTypes_ReturnsCorrectCount()
        {
            // Arrange
            var repository = CreateInstance();

            // Act
            var result = repository.GetProbabilityTypes();

            // Assert
            var enumValues = Enum.GetValues(typeof(ProbabilityType)).Cast<ProbabilityType>().ToList();
            Assert.Equal(enumValues.Count, result.Count());
        }

        [Fact]
        public void GetProbabilityTypes_ReturnsCorrectNames()
        {
            // Arrange
            var repository = CreateInstance();

            // Act
            var result = repository.GetProbabilityTypes();

            // Assert
            foreach (var enumValue in Enum.GetValues(typeof(ProbabilityType)).Cast<ProbabilityType>())
            {
                Assert.Contains(result, x => x.Name == enumValue.ToString());
            }
        }
    }
}