using CRC.Domain.Entities;

namespace CRC.Api.Models.Response;

public record MoradorResponse(
    int Id,
    CondominioResponse? Condominio,
    AuthResponse? Auth,
    string Cpf,
    string Nome,
    int? Pontos,
    int QtdMoradores,
    string IdentificadorRes
    );