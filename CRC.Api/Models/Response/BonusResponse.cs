namespace CRC.Api.Models.Response;

public record BonusResponse(
    int Id,
    CondominioResponse Condominio,
    string Nome,
    string? Descricao, 
    decimal Custo,
    int QtdMax
    );