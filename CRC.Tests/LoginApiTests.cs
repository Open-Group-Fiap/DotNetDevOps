using System.Net;
using System.Net.Http.Json;
using CIDA.Api;
using CRC.Api.Models.Request;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CRC.Tests;

[Collection("Test Collection")]
public class LoginApiTests
{
    
    private readonly HttpClient _client;

    public LoginApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task PostLogin_ReturnsUnauthorized_WhenEmailAndSenhaAreInvalid()
    {
        // Arrange
        var login = new AuthRequest(
            "example@example.com",
            "SenhaErrada"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/login", login);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PostLogin_ReturnsOk_WhenEmailAndSenhaAreValid()
    {
        // Arrange
        var login = new AuthRequest(
            "exemplo@exemplo.com",
            "123456"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/login", login);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

}