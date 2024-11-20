
namespace CRC.Api.Models.Request;

public record FaturaRequest(
    int IdMorador,
    int QtdConsumida,
    DateTime DtGeracao
    );