namespace CRC.Api.Models.Request;

public record MoradorBonusRequest(
    int IdMorador,
    int IdBonus,
    int Qtd
);  