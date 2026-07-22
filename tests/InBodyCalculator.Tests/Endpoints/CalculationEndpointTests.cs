using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace InBodyCalculator.Tests.Endpoints;

public sealed class CalculationEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CalculationEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
    }

    [Fact]
    public async Task CalculateBmi_ValidRequest_ReturnsExpectedJson()
    {
        // Arrange
        var request = new { height = 175.5, weight = 70 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/calculate/bmi", request);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(22.73, json.GetProperty("bmi").GetDouble());
        Assert.Equal("Normal", json.GetProperty("category").GetString());
    }

    [Fact]
    public async Task CalculateBmr_ValidRequest_ReturnsExpectedJson()
    {
        // Arrange
        var request = new { height = 175.5, weight = 70, age = 25, gender = "MALE" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/calculate/bmr", request);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(1676.88, json.GetProperty("bmr").GetDouble());
    }

    [Fact]
    public async Task CalculateTdee_ValidRequest_ReturnsExpectedJson()
    {
        // Arrange
        var request = new
        {
            height = 175.5,
            weight = 70,
            age = 25,
            gender = "male",
            activityLevel = "moderate"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/calculate/tdee", request);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(1676.88, json.GetProperty("bmr").GetDouble());
        Assert.Equal(2599.16, json.GetProperty("tdee").GetDouble());
    }

    [Theory]
    [InlineData("/api/calculate/bmi", "{\"height\":0,\"weight\":70}", "Height")]
    [InlineData("/api/calculate/bmi", "{\"height\":175.5,\"weight\":-1}", "Weight")]
    [InlineData("/api/calculate/bmi", "{}", "Height")]
    [InlineData("/api/calculate/bmr", "{\"height\":175.5,\"weight\":70,\"age\":25.5,\"gender\":\"male\"}", "Age")]
    [InlineData("/api/calculate/bmr", "{\"height\":175.5,\"weight\":70,\"age\":25}", "Gender")]
    [InlineData("/api/calculate/bmr", "{\"height\":175.5,\"weight\":70,\"age\":25,\"gender\":\"other\"}", "Gender")]
    [InlineData("/api/calculate/tdee", "{\"height\":175.5,\"weight\":70,\"age\":25,\"gender\":\"male\"}", "ActivityLevel")]
    [InlineData("/api/calculate/tdee", "{\"height\":175.5,\"weight\":70,\"age\":25,\"gender\":\"male\",\"activityLevel\":\"sometimes\"}", "ActivityLevel")]
    public async Task Calculate_InvalidRequest_ReturnsFieldValidationProblem(
        string path,
        string json,
        string expectedField)
    {
        // Arrange
        using var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(path, content);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.NotNull(problem);
        Assert.Equal(400, problem.Status);
        Assert.Contains(expectedField, problem.Errors.Keys);
    }
}