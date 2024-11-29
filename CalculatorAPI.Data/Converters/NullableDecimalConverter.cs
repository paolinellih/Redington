using System.Text.Json;
using System.Text.Json.Serialization;

public class NullableDecimalConverter : JsonConverter<decimal?>
{
    public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();
            
            // Handle the case where the string is empty or whitespace
            if (string.IsNullOrWhiteSpace(value))
            {
                return null; // Return null if it's an empty string or whitespace
            }
            // Handle the case where the string is a number
            if (decimal.TryParse(value, out var result))
            {
                return result; // Return number inside string
            }
        }

        // If the value is a valid decimal, return it
        if (reader.TokenType == JsonTokenType.Number && reader.TryGetDecimal(out var decimalValue))
        {
            return decimalValue;
        }

        // If it's an invalid decimal, throw an exception
        throw new JsonException("Invalid decimal format.");
    }

    public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
    {
        // Write either a null value or the actual decimal value
        if (value.HasValue)
        {
            writer.WriteNumberValue(value.Value);
        }
        else
        {
            writer.WriteNullValue(); // Write null if value is null
        }
    }
}