namespace CRC.Api.Models.Response;

public record AuthResponse(
    int Id,
    string Email,
    string HashSenha
    );