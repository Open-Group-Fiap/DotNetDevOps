using System.Net;
using System.Net.Http.Json;
using CIDA.Api;
using CRC.Api.Models.Request;
using CRC.Api.Models.Response;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CRC.Tests;

[Collection("Test Collection")]
public class CondominioTests
{
    private readonly HttpClient _client;
    
    public CondominioTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetCondominioById_ReturnsNotFound_WhenCondominioDoesNotExist()
    {
        // Arrange
        var id = 9999;

        // Act
        var response = await _client.GetAsync($"/condominio/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Condominio não encontrado", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostCondominio_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new CondominioRequest(
            "",
            ""
        );

        // Act
        var response = await _client.PostAsJsonAsync("/condominio", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostCondominio_ReturnsBadRequest_WhenNomeIsInvalid()
    {
        // Arrange
        var request = new CondominioRequest(
            "",
            "Endereco"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/condominio", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Nome do condominio inválido", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostCondominio_ReturnsBadRequest_WhenEnderecoIsInvalid()
    {
        // Arrange
        var request = new CondominioRequest(
            "Nome",
            ""
        );

        // Act
        var response = await _client.PostAsJsonAsync("/condominio", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Endereço inválido", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostCondominio_ReturnsBadRequest_WhenNomeIsTooShort()
    {
        // Arrange
        var request = new CondominioRequest(
            "Na",
            "Endereco"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/condominio", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Nome do condominio inválido", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostCondominio_ReturnsBadRequest_WhenEnderecoIsTooShort()
    {
        // Arrange
        var request = new CondominioRequest(
            "Nome",
            "En"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/condominio", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Endereço inválido", content.Trim('"'));
    }
    
    [Fact]
    public async Task PutCondominio_ReturnsNotFound_WhenCondominioDoesNotExist()
    {
        // Arrange
        var id = 9999;
        var request = new CondominioRequest(
            "Nome",
            "Endereco"
        );

        // Act
        var response = await _client.PutAsJsonAsync($"/condominio/{id}", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Condominio não encontrado", content.Trim('"'));
    }
    
    [Fact]
    public async Task PutCondominio_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var id = 1;
        var request = new CondominioRequest(
            "",
            ""
        );

        // Act
        var response = await _client.PutAsJsonAsync($"/condominio/{id}", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteCondominio_ReturnsNotFound_WhenCondominioDoesNotExist()
    {
        // Arrange
        var id = 9999;

        // Act
        var response = await _client.DeleteAsync($"/condominio/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Condominio não encontrado", content.Trim('"'));
    }
    
    [Fact]
    public async Task DeleteCondominio_ReturnsNoContent_WhenCondominioIsDeleted()
    {
        // Arrange
        var id = 2;

        // Act
        var response = await _client.DeleteAsync($"/condominio/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task GetAllCondominios_ReturnsOk_WhenCondominiosExist()
    {
        // Act
        var response = await _client.GetAsync("/condominio/list");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task PostCondominio_ReturnsCreated_WhenCondominioIsCreated()
    {
        // Arrange
        var request = new CondominioRequest(
            "Nome",
            "Endereco"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/condominio", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    
    [Fact]
    public async Task PutCondominio_ReturnsOk_WhenCondominioIsUpdated()
    {
        // Arrange
        var id = 1;
        var request = new CondominioRequest(
            "Nome",
            "Endereco"
        );

        // Act
        var response = await _client.PutAsJsonAsync($"/condominio/{id}", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task GetCondominioById_ReturnsOk_WhenCondominioExists()
    {
        // Arrange
        var id = 1;

        // Act
        var response = await _client.GetAsync($"/condominio/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
}