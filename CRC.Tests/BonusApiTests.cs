using System.Net;
using System.Net.Http.Json;
using CIDA.Api;
using CRC.Api.Models.Request;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CRC.Tests;

[Collection("Test Collection")]
public class BonusApiTests
{
    private readonly HttpClient _client;
    
    public BonusApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetBonusById_ReturnsNotFound_WhenBonusDoesNotExist()
    {
        // Arrange
        var id = 9999;

        // Act
        var response = await _client.GetAsync($"/bonus/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Bonus não encontrado", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostBonus_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new BonusRequest(
            0,
            "",
            "",
            0,
            0
        );

        // Act
        var response = await _client.PostAsJsonAsync("/bonus", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostBonus_ReturnsBadRequest_WhenNomeIsInvalid()
    {
        // Arrange
        var request = new BonusRequest(
            1,
            "",
            "Descricao",
            1,
            1
        );

        // Act
        var response = await _client.PostAsJsonAsync("/bonus", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Nome inválido", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostBonus_ReturnsBadRequest_WhenDescricaoIsInvalid()
    {
        // Arrange
        var request = new BonusRequest(
            1,
            "Nome",
            "",
            1,
            1
        );

        // Act
        var response = await _client.PostAsJsonAsync("/bonus", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Descrição inválida", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostBonus_ReturnsBadRequest_WhenCustoIsInvalid()
    {
        // Arrange
        var request = new BonusRequest(
            1,
            "Nome",
            "Descricao",
            0,
            1
        );

        // Act
        var response = await _client.PostAsJsonAsync("/bonus", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Valor inválido", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostBonus_ReturnsBadRequest_WhenIdCondominioIsInvalid()
    {
        // Arrange
        var request = new BonusRequest(
            0,
            "Nome",
            "Descricao",
            1,
            1
        );

        // Act
        var response = await _client.PostAsJsonAsync("/bonus", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Código de Condomínio inválido", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostBonus_ReturnsBadRequest_WhenQtdMaxIsInvalid()
    {
        // Arrange
        var request = new BonusRequest(
            1,
            "Nome",
            "Descricao",
            1,
            -1
        );

        // Act
        var response = await _client.PostAsJsonAsync("/bonus", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Quantidade máxima inválida", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostBonus_ReturnsCreated_WhenRequestIsValid()
    {
        // Arrange
        var request = new BonusRequest(
            1,
            "Nome",
            "Descricao",
            1,
            1
        );

        // Act
        var response = await _client.PostAsJsonAsync("/bonus", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    
    [Fact]
    public async Task PutBonus_ReturnsNotFound_WhenBonusDoesNotExist()
    {
        // Arrange
        var id = 9999;
        var request = new BonusRequest(
            1,
            "Nome",
            "Descricao",
            1,
            1
        );

        // Act
        var response = await _client.PutAsJsonAsync($"/bonus/{id}", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Bonus não encontrado", content.Trim('"'));
    }
    
    [Fact]
    public async Task PutBonus_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var id = 1;
        var request = new BonusRequest(
            1,
            "",
            "",
            0,
            0
        );

        // Act
        var response = await _client.PutAsJsonAsync($"/bonus/{id}", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PutBonus_ReturnsOk_WhenRequestIsValid()
    {
        // Arrange
        var id = 3;
        var request = new BonusRequest(
            1,
            "Nome",
            "Descricao",
            1,
            100
        );

        // Act
        var response = await _client.PutAsJsonAsync($"/bonus/{id}", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteBonus_ReturnsNotFound_WhenBonusDoesNotExist()
    {
        // Arrange
        var id = 9999;

        // Act
        var response = await _client.DeleteAsync($"/bonus/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Bonus não encontrado", content.Trim('"'));
    }
    
    [Fact]
    public async Task DeleteBonus_ReturnsNoContent_WhenRequestIsValid()
    {
        // Arrange
        var id = 2;

        // Act
        var response = await _client.DeleteAsync($"/bonus/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task GetBonusByCondominioId_ReturnsNotFound_WhenCondominioDoesNotExist()
    {
        // Arrange
        var idCondominio = 9999;

        // Act
        var response = await _client.GetAsync($"/bonus/condominio/{idCondominio}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Condomínio não encontrado", content.Trim('"'));
    }
    
    [Fact]
    public async Task GetBonusByCondominioId_ReturnsOk_WhenRequestIsValid()
    {
        // Arrange
        var idCondominio = 1;

        // Act
        var response = await _client.GetAsync($"/bonus/condominio/{idCondominio}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task GetAvaliableBonus_ReturnsNotFound_WhenCondominioDoesNotExist()
    {
        // Arrange
        var idCondominio = 9999;

        // Act
        var response = await _client.GetAsync($"/bonus/avaliable/condominio/{idCondominio}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Condomínio não encontrado", content.Trim('"'));
    }
    
    [Fact]
    public async Task GetAvaliableBonus_ReturnsOk_WhenRequestIsValid()
    {
        // Arrange
        var idCondominio = 1;

        // Act
        var response = await _client.GetAsync($"/bonus/avaliable/condominio/{idCondominio}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task GetAvaliableBonus_ReturnsNotFound_WhenBonusDoesNotExist()
    {
        // Arrange
        var id = 9999;

        // Act
        var response = await _client.GetAsync($"/bonus/avaliable/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Bonus não encontrado", content.Trim('"'));
    }
    
    [Fact]
    public async Task GetBonusById_ReturnsOk_WhenRequestIsValid()
    {
        // Arrange
        var id = 1;

        // Act
        var response = await _client.GetAsync($"/bonus/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}