namespace CRC.Api.Models.Response;

public record MoradorListResponse(
    int pageNumber,
    int pageSize,
    int total,
    IEnumerable<MoradorResponse> moradores
    );