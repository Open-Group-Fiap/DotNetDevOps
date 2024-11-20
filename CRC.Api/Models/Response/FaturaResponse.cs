namespace CRC.Api.Models.Response;

public record FaturaResponse(
    int Id,
    int IdMorador,
    int QtdConsumida,
    DateTime DtGeracao
    );