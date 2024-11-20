namespace CRC.Api.Models.Request;

public record MoradorRequest(
    int IdCondominio,
    string? Email,
    string? Senha,
    string Cpf,
    string Nome,
    int QtdMoradores,
    string IdentificadorRes
    );