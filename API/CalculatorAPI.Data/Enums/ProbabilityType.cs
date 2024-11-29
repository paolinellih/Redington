using System.Runtime.Serialization;

namespace CalculatorAPI.Data.Enums;

public enum ProbabilityType
{
    [EnumMember(Value = "Combined With")]
    CombinedWith,

    [EnumMember(Value = "Either")]
    Either,
}