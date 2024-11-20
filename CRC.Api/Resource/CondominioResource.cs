using CRC.Api.Models.Request;
using CRC.Api.Models.Response;
using CRC.Api.Service;
using CRC.Domain.Entities;

namespace CRC.Api.Resource;

public static class CondominioResource
{
    
    public static void MapCondominioEndpoints(this WebApplication app)
    {
        var condominioGroup = app.MapGroup("/condominio");
        
        condominioGroup.MapGet("/list", async (CondominioService _service, int pageNumber = 1, int pagesize = 30) =>
        {
            var result = await _service.GetAllAsync(pageNumber, pagesize);
            return Results.Ok(result);
        })
        .WithDescription("Retorna uma lista de condominios")
        .Produces<CondominioListResponse>(StatusCodes.Status200OK)
        .WithName("GetAllCondominio")
        .WithTags("Condominio");
        
        condominioGroup.MapGet("/{id}", async (CondominioService _service, int id) =>
        {
            var result = await _service.GetByIdAsync(id);
            return result != null ? Results.Ok(result) : Results.NotFound("Condominio não encontrado");
        })
        .WithDescription("Retorna um condominio pelo id")
        .Produces<CondominioResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithName("GetCondominioById")
        .WithTags("Condominio")
        .WithOpenApi(
            generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "Id do condominio a ser consultado";
                parameter.Required = true;
                return generatedOperation;
            }
        );
        
        condominioGroup.MapPost("/", async (CondominioRequest request, CondominioService _service) =>
        {
            if(string.IsNullOrWhiteSpace(request.Nome)) return Results.BadRequest("Nome do condominio inválido");
            if(string.IsNullOrWhiteSpace(request.Endereco)) return Results.BadRequest("Endereço inválido");
            if(request.Nome.Length < 3) return Results.BadRequest("Nome do condominio inválido");
            if(request.Endereco.Length < 3) return Results.BadRequest("Endereço inválido");
            
            
            var result = await _service.AddAsync(request);
            return Results.Created($"/condominio/{result.Id}", result);
        })
        .WithDescription("Cria um novo condominio")
        .Produces<CondominioResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithName("CreateCondominio")
        .WithTags("Condominio");
        
        condominioGroup.MapPut("/{id}", async (CondominioRequest request, CondominioService _service, int id) =>
        {
            if(string.IsNullOrWhiteSpace(request.Nome)) return Results.BadRequest("Nome do condominio inválido");
            if(string.IsNullOrWhiteSpace(request.Endereco)) return Results.BadRequest("Endereço inválido");
            if(request.Nome.Length < 3) return Results.BadRequest("Nome do condominio inválido");
            if(request.Endereco.Length < 3) return Results.BadRequest("Endereço inválido");
            
            var result = await _service.UpdateAsync(id, request);
            if (result == null) return Results.NotFound("Condominio não encontrado");
            return result != null ? Results.Ok(result) : Results.NotFound("Condominio não encontrado");
        })
        .WithDescription("Atualiza um condominio")
        .Produces<CondominioResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithName("UpdateCondominio")
        .WithTags("Condominio")
        .WithOpenApi(
            generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "Id do condominio a ser atualizado";
                parameter.Required = true;
                return generatedOperation;
            }
        );
        
        
        condominioGroup.MapDelete("/{id}", async (CondominioService _service, int id) =>
        {
            var result = await _service.DeleteAsync(id);
            return result != null ? Results.NoContent() : Results.NotFound("Condominio não encontrado");
        })
        .WithDescription("Deleta um condominio")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithName("DeleteCondominio")
        .WithTags("Condominio")
        .WithOpenApi(
            generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "Id do condominio a ser deletado";
                parameter.Required = true;
                return generatedOperation;
            }
        );
        
    } 
    
}