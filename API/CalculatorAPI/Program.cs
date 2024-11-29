using System.Text.Json;
using System.Text.Json.Serialization;
using CalculatorAPI.Business.ApiEndpoints;
using CalculatorAPI.Business.Factories.Services;
using CalculatorAPI.Business.Repositories;
using CalculatorAPI.Business.Services.General;
using CalculatorAPI.Data.Interfaces;
using CalculatorAPI.Data.Interfaces.Factories;
using CalculatorAPI.Data.Interfaces.Repositories;
using CalculatorAPI.Data.Interfaces.Services;
using CalculatorAPI.Data.Responses;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("https://0.0.0.0:44300", "http://0.0.0.0:5000"); // Bind to all IPs

builder.Services.AddCors();
builder.Services.AddHealthChecks();

// Configure global JSON options
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// For MVC if needed
builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add general services
// Logger
builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
builder.Services.AddScoped<ILogFileWriter, FileLogWriterService>();

// Add repositories
builder.Services.AddScoped<ICalculateProbabilityRepository, CalculateProbabilityRepository>();

// Add factories
builder.Services.AddSingleton<IProbabilityCalculatorFactory, ProbabilityCalculatorFactory>();


// Add the Process Services
AddProcessServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add Minimal Api hooks endpoints
app.AddCalculatorApiEndpoints();

// global cors policy
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true) // allow any origin
    .AllowCredentials()); // allow credentials

// Default basics up
app.MapGet("/whereami", () => Results.Ok($"Environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}, MachineName: {Environment.MachineName}, ProcessorCount: {Environment.ProcessorCount}"));
app.MapGet("/", () => Results.Ok());

// Global exception handler for JSON deserialization errors
app.UseExceptionHandler(options =>
{
    options.Run(async context =>
    {
        context.Response.StatusCode = 200; // Don't send Bad Request, in order to us to catch it and show
        context.Response.ContentType = "application/json";

        var errorResponse = new HttpProcessResult
        {
            Response = new ValidationErrorResponse
            {
                ValidationErrors = new Dictionary<string, List<string>>
                {
                    {"Request", ["Invalid request format. Please check your input values."]}
                },
                Successful = false
            }
        };
        
        await context.Response.WriteAsJsonAsync(errorResponse);
    });
});

app.Run();
return;

//Process Services
void AddProcessServices(IServiceCollection services)
{
    //--Calculator
    builder.Services.AddKeyedScoped<IHttpProcessService, CalculatorAPI.Business.Services.Process.Calculator.CalculateProbabilityService>(string.Concat("Calculator","CalculateProbabilityService"));
    builder.Services.AddKeyedScoped<IHttpProcessService, CalculatorAPI.Business.Services.Process.Calculator.GetProbabilityTypeService>(string.Concat("Calculator","GetProbabilityTypeService"));
    
}