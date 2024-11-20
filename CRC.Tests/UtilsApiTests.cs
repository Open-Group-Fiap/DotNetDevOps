using System.Net;
using CIDA.Api;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CRC.Tests;

[Collection("Test Collection")]
public class UtilsApiTests
{
    private readonly HttpClient _client;
    
    public UtilsApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetConsumo_ReturnsOk_WhenIdMoradorIsValidAndMoradorHaveTwoOrMoreFaturas()
    {
        // Arrange
        var idMorador = 1;

        // Act
        var response = await _client.GetAsync($"/consumo/{idMorador}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task GetConsumo_ReturnsBadRequest_WhenMoradorHaveLessThanTwoFaturas()
    {
        // Arrange
        var idMorador = 2;

        // Act
        var response = await _client.GetAsync($"/consumo/{idMorador}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task CreateARandomFatura_ReturnsOk_WhenMoradorExists()
    {
        // Arrange
        var idMorador = 1;

        // Act
        var response = await _client.GetAsync($"/randomFatura/{idMorador}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task CreateARandomFatura_ReturnsBadRequest_WhenMoradorDoesNotExist()
    {
        // Arrange
        var idMorador = 0;

        // Act
        var response = await _client.GetAsync($"/randomFatura/{idMorador}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
}