using System.Net;
using System.Net.Http.Json;
using CIDA.Api;
using CRC.Api.Models.Request;
using CRC.Api.Models.Response;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CRC.Tests;

[Collection("Test Collection")]
public class MoradorBonusApiTests
{
    private readonly HttpClient _client;
    
    public MoradorBonusApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetMoradorBonusById_ReturnsNotFound_WhenMoradorBonusDoesNotExist()
    {
        // Arrange
        var id = 9999;

        // Act
        var response = await _client.GetAsync($"/moradorbonus/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("MoradorBonus não encontrado", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostMoradorBonus_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new MoradorBonusRequest(
            0,
            0,
            -1
        );

        // Act
        var response = await _client.PostAsJsonAsync("/moradorbonus", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostMoradorBonus_ReturnsBadRequest_WhenIdMoradorIsInvalid()
    {
        // Arrange
        var request = new MoradorBonusRequest(
            0,
            1,
            1
        );

        // Act
        var response = await _client.PostAsJsonAsync("/moradorbonus", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Código de Morador inválido", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostMoradorBonus_ReturnsBadRequest_WhenIdBonusIsInvalid()
    {
        // Arrange
        var request = new MoradorBonusRequest(
            1,
            0,
            1
        );

        // Act
        var response = await _client.PostAsJsonAsync("/moradorbonus", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Código de Bonus inválido", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostMoradorBonus_ReturnsBadRequest_WhenQtdIsInvalid()
    {
        // Arrange
        var request = new MoradorBonusRequest(
            1,
            1,
            -1
        );

        // Act
        var response = await _client.PostAsJsonAsync("/moradorbonus", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Quantidade inválida", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostMoradorBonus_ReturnsBadRequest_WhenMoradorDoesNotExist()
    {
        // Arrange
        var request = new MoradorBonusRequest(
            9999,
            1,
            1
        );

        // Act
        var response = await _client.PostAsJsonAsync("/moradorbonus", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Morador não encontrado", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostMoradorBonus_ReturnsBadRequest_WhenBonusDoesNotExist()
    {
        // Arrange
        var request = new MoradorBonusRequest(
            1,
            9999,
            1
        );

        // Act
        var response = await _client.PostAsJsonAsync("/moradorbonus", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Bonus não encontrado", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostMoradorBonus_ReturnsBadRequest_WhenBonusIsNotAvailable()
    {
        // Arrange
        var request = new MoradorBonusRequest(
            1,
            3,
            9999
        );

        // Act
        var response = await _client.PostAsJsonAsync("/moradorbonus", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Quantidade de bonus indisponível", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostMoradorBonus_ReturnsCreated_WhenMoradorBonusIsCreated()
    {
        // Arrange
        var request = new MoradorBonusRequest(
            3,
            1,
            1
        );

        // Act
        var response = await _client.PostAsJsonAsync("/moradorbonus", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var content = await response.Content.ReadFromJsonAsync<MoradorBonusResponse>();
        Assert.NotNull(content);
    }
    
    [Fact]
    public async Task PutMoradorBonus_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new MoradorBonusRequest(
            0,
            0,
            -1
        );

        // Act
        var response = await _client.PutAsJsonAsync("/moradorbonus/1", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PutMoradorBonus_ReturnsOk_WhenMoradorBonusIsUpdated()
    {
        // Arrange
        var request = new MoradorBonusRequest(
            3,
            3,
            11
        );

        // Act
        var response = await _client.PutAsJsonAsync("/moradorbonus/3", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadFromJsonAsync<MoradorBonusResponse>();
        Assert.NotNull(content);
    }
    
    [Fact]
    public async Task DeleteMoradorBonus_ReturnsNotFound_WhenMoradorBonusDoesNotExist()
    {
        // Arrange
        var id = 9999;

        // Act
        var response = await _client.DeleteAsync($"/moradorbonus/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("MoradorBonus não encontrado", content.Trim('"'));
    }
    
    [Fact]
    public async Task DeleteMoradorBonus_ReturnsNoContent_WhenMoradorBonusIsDeleted()
    {
        // Arrange
        var id = 3;

        // Act
        var response = await _client.DeleteAsync($"/moradorbonus/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task GetMoradorBonusByMoradorId_ReturnsOk_WhenMoradorBonusExist()
    {
        // Arrange
        var id = 1;

        // Act
        var response = await _client.GetAsync($"/moradorbonus/morador/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadFromJsonAsync<IEnumerable<MoradorBonusResponse>>();
        Assert.NotNull(content);
    }
    
    [Fact]
    public async Task GetMoradorBonusByBonusId_ReturnsOk_WhenMoradorBonusExist()
    {
        // Arrange
        var id = 1;

        // Act
        var response = await _client.GetAsync($"/moradorbonus/bonus/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadFromJsonAsync<IEnumerable<MoradorBonusResponse>>();
        Assert.NotNull(content);
    }
    
    [Fact]
    public async Task GetAllMoradorBonus_ReturnsOk_WhenMoradorBonusExist()
    {
        // Act
        var response = await _client.GetAsync("/moradorbonus/list");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}