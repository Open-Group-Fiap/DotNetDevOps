namespace CRC.Api.Models.Response;

public record MetricsResponse(
    double RSquared,
    double RootMeanSquaredError
    );