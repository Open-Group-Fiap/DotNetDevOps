namespace CRC.Api.Models.Response;

public record CondominioListResponse(
    int PageNumber,
    int PageSize,
    int Total,
    IEnumerable<CondominioResponse> Condominios
    );