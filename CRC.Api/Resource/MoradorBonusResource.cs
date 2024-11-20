﻿using CRC.Api.Models.Request;
using CRC.Api.Models.Response;
using CRC.Api.Service;

namespace CRC.Api.Resource;

public static class MoradorBonusResource
{
    public static void MapMoradorBonusEndpoints(this WebApplication app)
    {
        var moradorBonusGroup = app.MapGroup("/moradorbonus");
        
        moradorBonusGroup.MapGet("/list", async (MoradorBonusService _service, int pageNumber = 1, int pageSize = 30) =>
            {
                var result = await _service.GetAllAsync(pageNumber, pageSize);
                return Results.Ok(result);
            })
            .WithDescription("Retorna uma lista de moradorbonus")
            .Produces<List<MoradorBonusResponse>>(StatusCodes.Status200OK)
            .WithName("GetAllMoradorBonus")
            .WithTags("MoradorBonus");
        
        
        moradorBonusGroup.MapGet("/{id:int}", async (MoradorBonusService _service, int id) =>
            {
                var result = await _service.GetByIdAsync(id);
                return result != null ? Results.Ok(result) : Results.NotFound("MoradorBonus não encontrado");
            })
            .WithDescription("Retorna um moradorbonus pelo id")
            .Produces<MoradorBonusResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("GetMoradorBonusById")
            .WithTags("MoradorBonus")
            .WithOpenApi(
                generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Id do moradorbonus a ser consultado";
                    parameter.Required = true;
                    return generatedOperation;
                }
            );
        
        moradorBonusGroup.MapPost("/", async (MoradorBonusService _service, MoradorService _serviceMorador, BonusService _serviceBonus, MoradorBonusRequest request) =>
            {
                if (request.IdMorador <= 0) return Results.BadRequest("Código de Morador inválido");
                if (request.IdBonus <= 0) return Results.BadRequest("Código de Bonus inválido");
                if (request.Qtd < 0) return Results.BadRequest("Quantidade inválida");
                
                var checkMorador = await _serviceMorador.GetByIdAsync(request.IdMorador);
                if (checkMorador == null)
                {
                    return Results.BadRequest("Morador não encontrado");
                }
                
                var checkBonus = await _serviceBonus.GetByIdAsync(request.IdBonus);
                if (checkBonus == null) return Results.BadRequest("Bonus não encontrado");

                var checkBonusAvailable = await _serviceBonus.GetAvaliableBonusAsync(request.IdBonus);
                if(checkBonusAvailable.Qtd < request.Qtd) return Results.BadRequest("Quantidade de bonus indisponível");
                
               
                if(checkBonus.Custo*request.Qtd > checkMorador.Pontos) return Results.BadRequest("Saldo insuficiente");
                
                var result = await _service.AddAsync(request);
                return Results.Created($"/moradorbonus/{result.Id}", result);
            })
            .WithDescription("Cria um novo moradorbonus")
            .Produces<MoradorBonusResponse>(StatusCodes.Status201Created)   
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("CreateMoradorBonus")
            .WithTags("MoradorBonus");
        
        moradorBonusGroup.MapPut("/{id:int}",
                async (MoradorBonusService _service, MoradorService _serviceMorador, BonusService _serviceBonus, MoradorBonusRequest request, int id) =>
                {
                    if (request.IdMorador <= 0) return Results.BadRequest("Código de Morador inválido");
                    if (request.IdBonus <= 0) return Results.BadRequest("Código de Bonus inválido");
                    if (request.Qtd < 0) return Results.BadRequest("Quantidade inválida");
                    
                    var checkMorador = await _serviceMorador.GetByIdAsync(request.IdMorador);
                    if (checkMorador == null)
                    {
                        return Results.BadRequest("Morador não encontrado");
                    }
                    
                    var checkBonus = await _serviceBonus.GetByIdAsync(request.IdBonus);
                    if (checkBonus == null)
                    {
                        return Results.BadRequest("Bonus não encontrado");
                    }

                    var checkMoradorBonus = await _service.GetByMoradorIdAndBonusIdAsync(request.IdMorador, request.IdBonus);
                    
                    if (checkMoradorBonus != null)
                    {                
                        var checkBonusAvailable = await _serviceBonus.GetAvaliableBonusAsync(request.IdBonus);
                        if(checkBonusAvailable.Qtd < request.Qtd-checkMoradorBonus.Qtd) return Results.BadRequest("Quantidade de bonus indisponível");
                        if ((checkBonus.Custo * request.Qtd) - checkMoradorBonus.Qtd*checkBonus.Custo > checkMorador.Pontos) return Results.BadRequest("Saldo insuficiente");
                        if (checkMoradorBonus.Qtd >= request.Qtd) return Results.BadRequest("Quantidade de bônus nova não pode ser menor que a anterior");
                    }

                    var result = await _service.UpdateAsync(id, request);
                    return Results.Ok(result);
                })
                .WithDescription("Atualiza um moradorbonus")
                .Produces<MoradorBonusResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithName("UpdateMoradorBonus")
                .WithTags("MoradorBonus");
        
        
        moradorBonusGroup.MapDelete("/{id:int}", async (MoradorBonusService _service, int id) =>
        {
            var result = await _service.DeleteAsync(id);
            return result != null ? Results.NoContent() : Results.NotFound("MoradorBonus não encontrado");
        })
        .WithDescription("Deleta um moradorbonus")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithName("DeleteMoradorBonus")
        .WithTags("MoradorBonus");
        
        moradorBonusGroup.MapGet("/morador/{id:int}", async (MoradorBonusService _service, int id) =>
            {
                var result = await _service.GetByMoradorIdAsync(id);
                return Results.Ok(result);
            })
            .WithDescription("Retorna uma lista de moradorbonus pelo id do morador")
            .Produces<IEnumerable<MoradorBonusResponse>>(StatusCodes.Status200OK)
            .WithName("GetMoradorBonusByMoradorId")
            .WithTags("MoradorBonus");
        
        moradorBonusGroup.MapGet("/bonus/{id:int}", async (MoradorBonusService _service, int id) =>
            {
                var result = await _service.GetByBonusIdAsync(id);
                return Results.Ok(result);
            })
            .WithDescription("Retorna uma lista de moradorbonus pelo id do bonus")
            .Produces<IEnumerable<MoradorBonusResponse>>()
            .WithName("GetMoradorBonusByBonusId")
            .WithTags("MoradorBonus");
        
    }
}