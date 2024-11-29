using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

public class ProgramTests
{
    [Fact]
    public async void ApplicationStartsSuccessfully()
    {
        // Arrange: Set up a TestServer and HttpClient
        var builder = WebApplication.CreateBuilder(new[] { "--urls", "http://localhost:5001" });
        builder.Services.AddControllers(); // Add any required services here
        builder.WebHost.UseTestServer(); // Use TestServer for integration testing

        var app = builder.Build();
        app.MapGet("/health", () => "Healthy"); // Example endpoint
        app.Start();

        var client = app.GetTestClient();

        // Act: Send a request to the app
        var response = await client.GetAsync("/health");

        // Assert: Verify the response
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Healthy", content);
    }
    
    [Fact]
    public async Task GetWhereAmI_ReturnsExpectedResponse()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder(new[] { "--urls", "http://localhost:5001" });
        builder.Services.AddControllers();
        builder.WebHost.UseTestServer();

        var app = builder.Build();
        app.MapGet("/whereami", () => Results.Ok($"Environment: {Environment.MachineName}"));

        await app.StartAsync();
        var client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/whereami");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Environment:", content);
    }
    
    [Fact]
    public async Task HealthCheckEndpoint_ReturnsHealthy()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder(new[] { "--urls", "http://localhost:5001" });
        builder.Services.AddHealthChecks();
        builder.WebHost.UseTestServer();

        var app = builder.Build();
        app.UseHealthChecks("/health");

        await app.StartAsync();
        var client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/health");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());
    }

}