using System.Reflection;
using System.Runtime.Serialization;

namespace CalculatorAPI.Business.Helper;
public static class EnumHelper
{
    // Get the custom string values associated with the EnumMember attribute
    public static string GetEnumValues<T>() where T : Enum
    {
        // Get all enum values
        var enumValues = Enum.GetValues(typeof(T)).Cast<Enum>();

        // Get the custom string value from EnumMember attribute
        var enumStrings = enumValues.Select(value => GetEnumMemberValue(value)).ToArray();

        return string.Join(", ", enumStrings);
    }

    // Helper method to retrieve the EnumMember custom string value or the Enum's name if no custom value exists
    private static string GetEnumMemberValue(Enum value)
    {
        var enumField = value.GetType().GetField(value.ToString());

        // Get the EnumMember attribute applied to the enum field
        var enumMemberAttribute = enumField.GetCustomAttribute<EnumMemberAttribute>();

        // Return the custom value if it exists, otherwise return the Enum name
        return enumMemberAttribute?.Value ?? value.ToString();
    }
}