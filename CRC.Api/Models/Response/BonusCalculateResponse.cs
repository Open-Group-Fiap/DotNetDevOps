namespace CRC.Api.Models.Response;

public record BonusCalculateResponse(
    int Id,
    string Nome,
    string? Descricao, 
    decimal Custo,
    int Qtd
    );