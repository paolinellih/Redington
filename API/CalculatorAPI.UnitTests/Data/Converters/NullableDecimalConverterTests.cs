using System.Text.Json;
using System.Text.Json.Serialization;
using CalculatorAPI.Data.Exceptions;
using Xunit;

namespace CalculatorAPI.UnitTests.Data.Converters;

public class NullableDecimalConverterTests
{
    private readonly JsonSerializerOptions _options;

    public NullableDecimalConverterTests()
    {
        _options = new JsonSerializerOptions
        {
            Converters = { new NullableDecimalConverter() },
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    [Fact]
    public void Read_ValidDecimalNumber_ReturnsDecimal()
    {
        var json = "123.45";
        var result = JsonSerializer.Deserialize<decimal?>(json, _options);

        Assert.NotNull(result);
        Assert.Equal(123.45m, result);
    }

    [Fact]
    public void Read_ValidDecimalString_ReturnsDecimal()
    {
        var json = "\"123.45\"";
        var result = JsonSerializer.Deserialize<decimal?>(json, _options);

        Assert.NotNull(result);
        Assert.Equal(123.45m, result);
    }

    [Fact]
    public void Read_EmptyString_ReturnsNull()
    {
        var json = "\"\"";
        var result = JsonSerializer.Deserialize<decimal?>(json, _options);

        Assert.Null(result);
    }

    [Fact]
    public void Read_WhitespaceString_ReturnsNull()
    {
        var json = "\"  \"";
        var result = JsonSerializer.Deserialize<decimal?>(json, _options);

        Assert.Null(result);
    }

    [Fact]
    public void Read_InvalidDecimal_ThrowsJsonException()
    {
        var json = "\"invalid\"";

        var exception = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<decimal?>(json, _options));

        Assert.Equal("Invalid decimal format.", exception.Message);
    }

    [Fact]
    public void Read_NullToken_ReturnsNull()
    {
        var json = "null";
        var result = JsonSerializer.Deserialize<decimal?>(json, _options);

        Assert.Null(result);
    }

    [Fact]
    public void Write_NullValue_WritesNullToken()
    {
        decimal? value = null;
        var json = JsonSerializer.Serialize(value, _options);

        Assert.Equal("null", json);
    }

    [Fact]
    public void Write_DecimalValue_WritesDecimalNumber()
    {
        decimal? value = 123.45m;
        var json = JsonSerializer.Serialize(value, _options);

        Assert.Equal("123.45", json);
    }
    
    [Fact]
    public void Write_NullValue_CallsWriteNullValue()
    {
        // Arrange
        decimal? value = null;
        var memoryStream = new MemoryStream();
        var jsonWriter = new Utf8JsonWriter(memoryStream, new JsonWriterOptions { SkipValidation = true });

        var converter = new NullableDecimalConverter();

        // Act
        converter.Write(jsonWriter, value, _options);

        // Assert
        jsonWriter.Flush(); // Ensure that the data is written to the memory stream
        var result = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());

        // Check if the output is "null" as expected for a null value
        Assert.Equal("null", result); 
    }
    
    [Fact]
    public void DefaultConstructor_SetsCorrectMessage()
    {
        // Act
        var exception = new NoContentFoundException();

        // Assert
        Assert.Equal("No content found.", exception.Message);
    }

    [Fact]
    public void Constructor_WithCustomMessage_SetsMessageCorrectly()
    {
        // Arrange
        var customMessage = "Custom message";

        // Act
        var exception = new NoContentFoundException(customMessage);

        // Assert
        Assert.Equal(customMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithCustomMessageAndInnerException_SetsCorrectProperties()
    {
        // Arrange
        var customMessage = "Custom message with inner exception";
        var innerException = new Exception("Inner exception");

        // Act
        var exception = new NoContentFoundException(customMessage, innerException);

        // Assert
        Assert.Equal(customMessage, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
}
