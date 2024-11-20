namespace CRC.Api.Models.Response;

public record BonusListResponse(
    int PageNumber,
    int PageSize,
    int Total,
    IEnumerable<BonusResponse> Bonus
    );