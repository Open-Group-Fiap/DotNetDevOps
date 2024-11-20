namespace CRC.Api.Models.Response;

public record FaturaListResponse(
    int PageNumber,
    int PageSize,
    int Total,
    IEnumerable<FaturaResponse> Faturas
    );