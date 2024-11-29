using System.Text.Json.Serialization;
using CalculatorAPI.Data.Enums;
using CalculatorAPI.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorAPI.Data.Requests.Calculator;

public class CalculateProbabilityRequest : IRequest
{
    [JsonConverter(typeof(NullableDecimalConverter))]
    public decimal? A { get; set; } 
    
    [JsonConverter(typeof(NullableDecimalConverter))]
    public decimal? B { get; set; }
    public ProbabilityType? Type { get; set; }
}