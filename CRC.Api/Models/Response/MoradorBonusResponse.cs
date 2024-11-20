namespace CRC.Api.Models.Response;

public record MoradorBonusResponse(
    int Id,
    int IdMorador,
    int IdBonus,
    int Qtd
    );