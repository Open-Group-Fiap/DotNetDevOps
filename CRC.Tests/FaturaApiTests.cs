using System.Net;
using System.Net.Http.Json;
using CIDA.Api;
using CRC.Api.Models.Request;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CRC.Tests;

[Collection("Test Collection")]
public class FaturaApiTests
{
    private readonly HttpClient _client;
    
    public FaturaApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetFaturaById_ReturnsNotFound_WhenFaturaDoesNotExist()
    {
        // Arrange
        var id = 9999;

        // Act
        var response = await _client.GetAsync($"/fatura/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Fatura não encontrada", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostFatura_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new FaturaRequest(
            0,
            0,
            DateTime.Now
        );

        // Act
        var response = await _client.PostAsJsonAsync("/fatura", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostFatura_ReturnsBadRequest_WhenIdMoradorIsInvalid()
    {
        // Arrange
        var request = new FaturaRequest(
            0,
            1,
            DateTime.Now
        );

        // Act
        var response = await _client.PostAsJsonAsync("/fatura", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostFatura_ReturnsBadRequest_WhenMoradorDoesNotExist()
    {
        // Arrange
        var request = new FaturaRequest(
            9999,
            1,
            DateTime.Now
        );

        // Act
        var response = await _client.PostAsJsonAsync("/fatura", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PutFatura_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new FaturaRequest(
            0,
            0,
            DateTime.Now
        );

        // Act
        var response = await _client.PutAsJsonAsync("/fatura/1", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PutFatura_ReturnsBadRequest_WhenMoradorDoesNotExist()
    {
        // Arrange
        var request = new FaturaRequest(
            9999,
            1,
            DateTime.Now
        );

        // Act
        var response = await _client.PutAsJsonAsync("/fatura/1", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PutFatura_ReturnsNotFound_WhenFaturaDoesNotExist()
    {
        // Arrange
        var request = new FaturaRequest(
            1,
            1,
            DateTime.Now
        );

        // Act
        var response = await _client.PutAsJsonAsync("/fatura/9999", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteFatura_ReturnsNotFound_WhenFaturaDoesNotExist()
    {
        // Arrange
        var id = 9999;

        // Act
        var response = await _client.DeleteAsync($"/fatura/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteFatura_ReturnsNoContent_WhenFaturaIsDeleted()
    {
        // Arrange
        var id = 2;

        // Act
        var response = await _client.DeleteAsync($"/fatura/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task GetFaturaByMoradorId_ReturnsOk_WhenMoradorExists()
    {
        // Arrange
        var idMorador = 1;

        // Act
        var response = await _client.GetAsync($"/fatura/morador/{idMorador}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task GetFaturaById_ReturnsOk_WhenFaturaExists()
    {
        // Arrange
        var id = 1;

        // Act
        var response = await _client.GetAsync($"/fatura/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task PostFatura_ReturnsCreated_WhenFaturaIsCreated()
    {
        // Arrange
        var request = new FaturaRequest(
            1,
            100,
            DateTime.Now
        );

        // Act
        var response = await _client.PostAsJsonAsync("/fatura", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    
    [Fact]
    public async Task PutFatura_ReturnsOk_WhenFaturaIsUpdated()
    {
        // Arrange
        var request = new FaturaRequest(
            1,
            100,
            DateTime.Now
        );

        // Act
        var response = await _client.PutAsJsonAsync("/fatura/3", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task GetAllFaturas_ReturnsOk_WhenFaturasExists()
    {
        // Act
        var response = await _client.GetAsync("/fatura/list");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    
    
    
    
}