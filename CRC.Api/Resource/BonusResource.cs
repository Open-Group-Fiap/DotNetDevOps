using CRC.Api.Models.Request;
using CRC.Api.Models.Response;
using CRC.Api.Service;

namespace CRC.Api.Resource;

public static class BonusResource
{
    public static void MapBonusEndpoints(this WebApplication app)
    {
        var bonusGroup = app.MapGroup("/bonus");

        bonusGroup.MapGet("/list", async (BonusService _service, int pageNumber = 1, int pageSize = 30) =>
            {
                var result = await _service.GetAllAsync(pageNumber, pageSize);
                return Results.Ok(result);
            })
            .WithDescription("Retorna uma lista de bonus")
            .Produces<BonusListResponse>(StatusCodes.Status200OK)
            .WithName("GetAllBonus")
            .WithTags("Bonus");

        bonusGroup.MapGet("/{id:int}", async (BonusService _service, int id) =>
            {
                var result = await _service.GetByIdAsync(id);
                return result != null ? Results.Ok(result) : Results.NotFound("Bonus não encontrado");
            })
            .WithDescription("Retorna um bonus pelo id")
            .Produces<BonusResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("GetBonusById")
            .WithTags("Bonus")
            .WithOpenApi(
                generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Id do bonus a ser consultado";
                    parameter.Required = true;
                    return generatedOperation;
                }
            );

        bonusGroup.MapPost("/", async (BonusService _service, CondominioService _serviceCon, BonusRequest request) =>
            {
                var checkCondominio = await _serviceCon.GetByIdAsync(request.IdCondominio);
                if (checkCondominio == null)
                {
                    return Results.BadRequest("Código de Condomínio inválido");
                }

                if (string.IsNullOrWhiteSpace(request.Nome)) return Results.BadRequest("Nome inválido");
                if (string.IsNullOrWhiteSpace(request.Descricao)) return Results.BadRequest("Descrição inválida");
                if (request.Custo <= 0) return Results.BadRequest("Valor inválido");
                if (request.IdCondominio <= 0) return Results.BadRequest("Código de Condomínio inválido");
                if (request.QtdMax < 0) return Results.BadRequest("Quantidade máxima inválida");
                if (request.Descricao.Length < 3) return Results.BadRequest("Descrição inválida");
                if (request.Nome.Length < 3) return Results.BadRequest("Nome inválido");


                var result = await _service.AddAsync(request);
                return Results.Created($"/bonus/{result.Id}", result);
            })
            .WithDescription("Cria um novo bonus")
            .Produces<BonusResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("CreateBonus")
            .WithTags("Bonus");

        bonusGroup.MapPut("/{id:int}",
                async (BonusService _service, CondominioService _serviceCon, BonusRequest request, int id) =>
                {
                    var checkCondominio = await _serviceCon.GetByIdAsync(request.IdCondominio);
                    if (checkCondominio == null)
                    {
                        return Results.BadRequest("Código de Condomínio inválido");
                    }

                    if (string.IsNullOrWhiteSpace(request.Nome)) return Results.BadRequest("Nome inválido");
                    if (string.IsNullOrWhiteSpace(request.Descricao)) return Results.BadRequest("Descrição inválida");
                    if (request.Custo <= 0) return Results.BadRequest("Valor inválido");
                    if (request.IdCondominio <= 0) return Results.BadRequest("Código de Condomínio inválido");
                    if (request.QtdMax < 0) return Results.BadRequest("Quantidade máxima inválida");
                    if (request.Descricao.Length < 3) return Results.BadRequest("Descrição inválida");
                    if (request.Nome.Length < 3) return Results.BadRequest("Nome inválido");

                    var result = await _service.UpdateAsync(id, request);
                    if (result == null) return Results.NotFound("Bonus não encontrado");
                    return result != null ? Results.Ok(result) : Results.NotFound("Bonus não encontrado");
                })
            .WithDescription("Atualiza um bonus")
            .Produces<BonusResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("UpdateBonus")
            .WithTags("Bonus");

        bonusGroup.MapDelete("/{id:int}", async (BonusService _service, int id) =>
            {
                var result = await _service.DeleteAsync(id);
                return result != null ? Results.NoContent() : Results.NotFound("Bonus não encontrado");
            })
            .WithDescription("Deleta um bonus")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("DeleteBonus")
            .WithTags("Bonus");

        bonusGroup.MapGet("/condominio/{idCondominio:int}",
                async (BonusService _service, CondominioService _serviceCon, int idCondominio, int pageNumber = 1,
                    int pageSize = 30) =>
                {
                    var checkCondominio = await _serviceCon.GetByIdAsync(idCondominio);
                    if (checkCondominio == null)
                    {
                        return Results.NotFound("Condomínio não encontrado");
                    }

                    var result = await _service.GetByCondominioIdAsync(idCondominio, pageNumber, pageSize);
                    return Results.Ok(result);
                })
            .WithDescription("Retorna uma lista de bonus por condomínio")
            .Produces<BonusListResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("GetBonusByCondominioId")
            .WithTags("Bonus")
            .WithOpenApi(generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "Id do condomínio a qual os bonus a ser consultados pertencem";
                parameter.Required = true;
                return generatedOperation;
            });

        bonusGroup.MapGet("/avaliable/condominio/{idCondominio:int}",
                async (BonusService _service, CondominioService _serviceCon, int idCondominio) =>
                {
                    var checkCondominio = await _serviceCon.GetByIdAsync(idCondominio);
                    if (checkCondominio == null)
                    {
                        return Results.NotFound("Condomínio não encontrado");
                    }

                    var result = await _service.GetAvaliableByCondominioIdAsync(idCondominio);
                    return Results.Ok(result);
                })
            .WithDescription("Retorna uma lista de bonus disponíveis")
            .Produces<IEnumerable<BonusCalculateResponse>>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("GetAvaliableBonus")
            .WithTags("Bonus")
            .WithOpenApi(generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "Id do condomínio a qual os bonus a ser consultados pertencem";
                parameter.Required = true;
                return generatedOperation;
            });

        bonusGroup.MapGet("/avaliable/{id:int}",
                async (BonusService _service, int id) =>
                {
                    var result = await _service.GetAvaliableBonusAsync(id);
                    return result != null ? Results.Ok(result) : Results.NotFound("Bonus não encontrado");
                })
            .WithDescription("Calcula a quantidade de bonus disponíveis")
            .Produces<BonusCalculateResponse>()
            .WithName("CalculateBonus")
            .WithTags("Bonus")
            .WithOpenApi(generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "Id do bonus a ser consultados";
                parameter.Required = true;
                return generatedOperation;
            });



    }
    
}