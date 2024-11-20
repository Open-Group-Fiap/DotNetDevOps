using CRC.Api.Models.Request;
using CRC.Api.Models.Response;
using CRC.IA.Entities;
using CRC.IA.Service;

namespace CRC.Api.Resource;

public static class PrevisaoResource
{
    public static void MapPrevisaoEndpoints(this WebApplication app)
    {
        var previsaoGroup = app.MapGroup("/previsao");

        previsaoGroup.MapPost("/", async (PrevisaoRequest model) =>
        {

            var possiblesRegioes = new List<string>(["Sul", "Sudeste", "Centro-Oeste", "Norte", "Nordeste"]);


            if (model.QtdMoradores <= 0) return Results.BadRequest("Quantidade de moradores inválida. A quantidade de moradores deve ser maior que 0");
            if(!possiblesRegioes.Contains(model.Regiao)) return Results.BadRequest("Região inválida. Use: Sul, Sudeste, Centro-Oeste, Norte ou Nordeste");

            var data = new FaturaData
            {
                QtdMoradores = model.QtdMoradores,
                Regiao = model.Regiao
            };

            var prediction = MLModelService.PredictQtdConsumida(data);


            var previsaoRound = (int)Math.Round(prediction.QtdConsumida);
            
            var reponse = new PrevisaoResponse(previsaoRound);

                return Results.Ok(reponse);
           
        })
            .Accepts<PrevisaoRequest>("application/json")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("Previsão")
            .WithTags("IA")
            .WithDescription("Gere a previsão de consumo de energia em KWH de uma residência")
            .WithOpenApi();

        previsaoGroup.MapGet("/metrics", () =>
        {
            var (RSquared, RootMeanSquaredError) = MLModelService.EvaluateModel();

            return new MetricsResponse(RSquared, RootMeanSquaredError);
        })
            .Produces<MetricsResponse>()
            .WithName("Metrics")
            .WithTags("IA")
            .WithDescription("Retorna as métricas de avaliação do modelo de IA (R2 e RMSE)")
            .WithOpenApi();

    }
}