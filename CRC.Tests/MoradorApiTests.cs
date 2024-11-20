using System.Net;
using System.Net.Http.Json;
using CIDA.Api;
using CRC.Api.Models.Request;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CRC.Tests;

[Collection("Test Collection")]
public class MoradorApiTests
{
    private readonly HttpClient _client;

    public MoradorApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetMoradorById_ReturnsNotFound_WhenMoradorDoesNotExist()
    {
        // Arrange
        var id = 9999;

        // Act
        var response = await _client.GetAsync($"/morador/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Morador não encontrado", content.Trim('"'));
    }
    
    [Fact]
    public async Task PostMorador_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new MoradorRequest(
            0,
           "",
             "",
            "",
            "",
            0,
            ""
        );

        // Act
        var response = await _client.PostAsJsonAsync("/morador", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostMorador_ReturnsBadRequest_WhenEmailIsInvalid()
    {
        // Arrange
        var request = new MoradorRequest(
            1,
            "email",
            "123456Aa!",
            "000.000.000-00",
            "Nome",
            1,
            "Identificador"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/morador", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Email inválido", content.Trim('"'));
    }

    [Fact]
    public async Task PostMorador_ReturnsBadRequest_WhenCpfIsInvalid()
    {
        // Arrange
        var request = new MoradorRequest(
            1,
            "example@example.com",
            "123456Aa!",
            "000.000.000-0",
            "Nome",
            2,
            "Identificador"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/morador", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("CPF inválido", content.Trim('"'));
    }

    [Fact]
    public async Task PostMorador_ReturnsBadRequest_WhenCondominioIdIsInvalid()
    {
        // Arrange
        var request = new MoradorRequest(
            0,
            "example@example.com",
            "123456Aa!",
            "000.000.000-00",
            "Nome",
            2,
            "Identificador"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/morador", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Código de Condomínio inválido", content.Trim('"'));
    }

    [Fact]
    public async Task PostMorador_ReturnsBadRequest_WhenQtdMoradoresIsInvalid()
    {
        // Arrange
        var request = new MoradorRequest(
            1,
            "example@example.com",
            "123456Aa!",
            "000.000.000-00",
            "Nome",
            0,
            "Identificador"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/morador", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Quantidade de moradores inválida", content.Trim('"'));
    }

    [Fact]
    public async Task PostMorador_ReturnsBadRequest_WhenIdentificadorResIsInvalid()
    {
        // Arrange
        var request = new MoradorRequest(
            1,
            "example@example.com",
            "123456Aa!",
            "000.000.000-00",
            "Nome",
            4,
            ""
        );

        // Act
        var response = await _client.PostAsJsonAsync("/morador", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Identificador de residência inválido", content.Trim('"'));
    }

    [Fact]
    public async Task PostMorador_ReturnsBadRequest_WhenEmailAlreadyExists()
    {
        // Arrange
        var request = new MoradorRequest(
            1,
            "exemplo@exemplo.com",
            "123456Aa!",
            "000.000.000-00",
            "Nome",
            4,
            "Residencial"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/morador", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Email já cadastrado", content.Trim('"'));
    }

    [Fact]
    public async Task PostMorador_ReturnsBadRequest_WhenCpfAlreadyExists()
    {
        // Arrange
        var request = new MoradorRequest(
            1,
            "example@example.com",
            "123456Aa!",
            "123.456.789-01",
            "Nome",
            2,
            "Residencial"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/morador", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("CPF já cadastrado", content.Trim('"'));
    }

    [Fact]
    public async Task PostMorador_ReturnsCreated_WhenRequestIsValid()
    {
        // Arrange
        var request = new MoradorRequest(
            1,
            "example123@example.com",
            "123456Aa!",
            "000.000.000-11",
            "Nome",
            2,
            "Residencial"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/morador", request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    
    [Fact]
    public async Task GetMoradorById_ReturnsOk_WhenMoradorExists()
    {
        // Arrange
        var id = 1;

        // Act
        var response = await _client.GetAsync($"/morador/{id}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task PutMorador_ReturnsMorador_WhenMoradorExists()
    {
        // Arrange
        var request = new MoradorRequest(
            1,
            "leo@email.com",
            "123456Aa!",
            "418.418.418-18",
            "Leonardo",
            2,
            "Residencia 1A"
        );

        // Act
        var response = await _client.PutAsJsonAsync("/morador/3", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PutMorador_ReturnsNotFound_WhenMoradorDoesNotExist()
    {
        // Arrange
        var request = new MoradorRequest(
            1,
            "leo@email.com",
            "123456Aa!",
            "418.418.418-18",
            "Leonardo",
            2,
            "Residencia 1A"
        );

        // Act
        var response = await _client.PutAsJsonAsync("/morador/9999", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }
    
    [Fact]
    public async Task DeleteMorador_ReturnsNotFound_WhenMoradorDoesNotExist()
    {
        // Arrange
        var id = 9999;

        // Act
        var response = await _client.DeleteAsync($"/morador/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteMorador_ReturnsNoContent_WhenMoradorExists()
    {
        // Arrange
        var id = 2;

        // Act
        var response = await _client.DeleteAsync($"/morador/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task GetMoradorByEmail_ReturnsNotFound_WhenMoradorDoesNotExist()
    {
        // Arrange
        var email = "edpajowidj@dhjaohowd.com";

        // Act
        var response = await _client.GetAsync($"/morador/email/{email}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Morador não encontrado", content.Trim('"'));
    }

    [Fact]
    public async Task GetMoradorByEmail_ReturnsOk_WhenMoradorExists()
    {
        // Arrange
        var email = "exemplo@exemplo.com";

        // Act
        var response = await _client.GetAsync($"/morador/email/{email}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


}