using CRC.Api.Models.Request;
using CRC.Api.Models.Response;
using CRC.Api.Service;
using CRC.Api.Utils;

namespace CRC.Api.Resource;

public static class FaturaResource
{
    public static void MapFaturaEndpoints(this WebApplication app)
    {
        var faturaGroup = app.MapGroup("/fatura");
        
        faturaGroup.MapGet("/list", async (FaturaService _service, int pageNumber = 1, int pagesize = 30) =>
        {
            var result = await _service.GetAllAsync(pageNumber, pagesize);
            return Results.Ok(result);
        })
        .WithDescription("Retorna uma lista de faturas")
        .Produces<FaturaListResponse>(StatusCodes.Status200OK)
        .WithName("GetAllFatura")
        .WithTags("Fatura");
        
        faturaGroup.MapGet("/{id}", async (FaturaService _service, int id) =>
        {
            var result = await _service.GetByIdAsync(id);
            return result != null ? Results.Ok(result) : Results.NotFound("Fatura não encontrada");
        })
        .WithDescription("Retorna uma fatura pelo id")
        .Produces<FaturaResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithName("GetFaturaById")
        .WithTags("Fatura")
        .WithOpenApi(
            generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "Id da fatura a ser consultada";
                parameter.Required = true;
                return generatedOperation;
            }
        );
        
        faturaGroup.MapPost("/", async (FaturaRequest request, FaturaService _service, MoradorService _moradorService) =>
        {
            if(request.QtdConsumida <= 0) return Results.BadRequest("Quantidade consumida inválido");
            var checkMorador = await _moradorService.GetByIdAsync(request.IdMorador);
            if(checkMorador == null) return Results.BadRequest("Morador não encontrado");
            
            
            
            var result = await _service.AddAsync(request);
            return Results.Created($"/fatura/{result.Id}", result);
        })
        .WithDescription("Cria uma nova fatura")
        .Produces<FaturaResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithName("CreateFatura")
        .WithTags("Fatura");
        
        faturaGroup.MapPut("/{id}", async (FaturaRequest request, FaturaService _service, int id, MoradorService _moradorService) =>
        {
            if(request.QtdConsumida <= 0) return Results.BadRequest("Quantidade consumida inválido");
            var checkMorador = await _moradorService.GetByIdAsync(request.IdMorador);
            if(checkMorador == null) return Results.BadRequest("Morador não encontrado");

            var result = await _service.UpdateAsync(id, request);
            return result != null ? Results.Ok(result) : Results.NotFound("Fatura não encontrada");
        })
        .WithDescription("Atualiza uma fatura")
        .Produces<FaturaResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithName("UpdateFatura")
        .WithTags("Fatura");
        
        faturaGroup.MapDelete("/{id}", async (FaturaService _service, int id) =>
        {
            var result = await _service.DeleteAsync(id);
            return result != null ? Results.NoContent() : Results.NotFound("Fatura não encontrada");
        })
        .WithDescription("Deleta uma fatura")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithName("DeleteFatura")
        .WithTags("Fatura")
        .WithOpenApi(
            generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "Id da fatura a ser deletada";
                parameter.Required = true;
                return generatedOperation;
            }
        );
        
        faturaGroup.MapGet("/morador/{idMorador:int}", async (FaturaService _service, int idMorador, int pageNumber = 1, int pagesize = 30) =>
        {
            var result = await _service.GetByMoradorAsync(idMorador, pageNumber, pagesize);
            return Results.Ok(result);
        })
        .WithDescription("Retorna uma lista de faturas de um morador")
        .Produces<FaturaListResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithName("GetFaturaByMorador")
        .WithTags("Fatura")
        .WithOpenApi(
            generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "Id do morador a ser consultado";
                parameter.Required = true;
                return generatedOperation;
            }
        );
    }
}