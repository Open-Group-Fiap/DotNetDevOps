namespace CRC.Api.Models.Request;

public record BonusRequest(
    int IdCondominio,
    string Nome,
    string? Descricao, 
    decimal Custo,
    int QtdMax
    );