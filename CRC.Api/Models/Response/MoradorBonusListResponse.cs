namespace CRC.Api.Models.Response;

public record MoradorBonusListResponse(
    int PageSize,
    int PageNumber,
    int Total,
    IEnumerable<MoradorBonusResponse> MoradorBonus);