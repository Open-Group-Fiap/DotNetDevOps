using System.Net;
using System.Net.Http.Json;
using CIDA.Api;
using CRC.Api.Models.Request;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CRC.Tests;

[Collection("Test Collection")]
public class PrevisaoApiTests
{
    private readonly HttpClient _client;
    
    public PrevisaoApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task PostPrevisao_ReturnsBadRequest_WhenQtdMoradoresIsInvalid()
    {
        // Arrange
        var request = new PrevisaoRequest(
            0,
            "Sul"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/previsao", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostPrevisao_ReturnsBadRequest_WhenRegiaoIsInvalid()
    {
        // Arrange
        var request = new PrevisaoRequest(
            1,
            "Suldoeste"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/previsao", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostPrevisao_ReturnsOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new PrevisaoRequest(
            1,
            "Sul"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/previsao", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task GetMetrics_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/previsao/metrics");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
}