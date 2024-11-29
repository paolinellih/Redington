using CalculatorAPI.Data.Enums;

namespace CalculatorAPI.Business.Helper;

public static class EnumHelper
{
    public static string GetEnumValues<T>() where T : Enum
    {
        return string.Join(", ", Enum.GetNames(typeof(T)).Where(e => !e.Equals(ProbabilityType.SelectAnOption.ToString())));
    }
}