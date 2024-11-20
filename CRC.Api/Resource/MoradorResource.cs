using CRC.Api.Models.Request;
using CRC.Api.Models.Response;
using CRC.Api.Service;
using CRC.Api.Utils;

namespace CRC.Api.Resource;

public static class MoradorResource
{
    public static void MapMoradorEndpoints(this WebApplication app)
    {
        var moradorGroup = app.MapGroup("/morador");


        moradorGroup.MapGet("/{id:int}", async (MoradorService _service, int id) =>
            {
                var result = await _service.GetByIdAsync(id);
                return result != null ? Results.Ok(result) : Results.NotFound("Morador não encontrado");
            })
            .WithDescription("Retorna um morador pelo id")
            .Produces<MoradorResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("GetMoradorById")
            .WithTags("Morador")
            .WithOpenApi(
                generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Id do insight a ser consultado";
                    parameter.Required = true;
                    return generatedOperation;
                }
            );


        moradorGroup.MapPost("/", async (MoradorService _service,  CondominioService _serviceCon, MoradorRequest request) =>
            {
                if (request.Email != null) request = request with { Email = request.Email.Trim() };
                if(request.Senha != null) request = request with { Senha = request.Senha.Trim() };
                
                if(string.IsNullOrWhiteSpace(request.Nome)) return Results.BadRequest("Nome inválido");
                if(string.IsNullOrWhiteSpace(request.Email)) return Results.BadRequest("Email inválido");
                if(string.IsNullOrWhiteSpace(request.Cpf)) return Results.BadRequest("CPF inválido, use o formato 000.000.000-00");
                if(request.IdCondominio <= 0) return Results.BadRequest("Código de Condomínio inválido");
                if(request.QtdMoradores <= 0) return Results.BadRequest("Quantidade de moradores inválida");
                if(request.IdentificadorRes.Length < 3) return Results.BadRequest("Identificador de residência inválido");
                if(!UtilsService.IsValidEmail(request.Email)) return Results.BadRequest("Email inválido");
                if(!UtilsService.IsValidCpf(request.Cpf)) return Results.BadRequest("CPF inválido");
                //check if HashSenha contain at least 8 characters, 1 uppercase, 1 lowercase, 1 number and 1 special character
                if(!UtilsService.IsValidPassword(request.Senha)) return Results.BadRequest("Senha inválida, deve conter no mínimo 8 caracteres, 1 letra maiúscula, 1 letra minúscula, 1 número e 1 caractere especial");
                

                var checkUserEmail = await _service.GetByEmailAsync(request.Email);
                
                if(checkUserEmail != null )
                {
                    if (checkUserEmail.Auth.Email == request.Email)
                    {
                        return Results.BadRequest("Email já cadastrado");
                    }
                }
                
                var checkUserCpf = await _service.GetByCpfAsync(request.Cpf);

                if(checkUserCpf != null)
                {
                    if (checkUserCpf.Cpf == request.Cpf)
                    {
                        return Results.BadRequest("CPF já cadastrado");
                    }
                }
                
                var checkCondominio = await _serviceCon.GetByIdAsync(request.IdCondominio);
                if(checkCondominio == null) return Results.BadRequest("Código de Condomínio inválido");

                var result = await _service.AddAsync(request);
                return Results.Created($"/morador/{result.Id}", result);
            })
            .WithDescription("Cria um novo morador")
            .Produces<MoradorResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("CreateMorador")
            .WithTags("Morador");
        
        moradorGroup.MapPut("/{id:int}", async (MoradorService _service, CondominioService _serviceCon, MoradorRequest request, int id) =>
        {
            if (request.Email != null) request = request with { Email = request.Email.Trim() };
            if(request.Senha != null) request = request with { Senha = request.Senha.Trim() };
                
            if(string.IsNullOrWhiteSpace(request.Nome)) return Results.BadRequest("Nome inválido");
            if(string.IsNullOrWhiteSpace(request.Email)) return Results.BadRequest("Email inválido");
            if(string.IsNullOrWhiteSpace(request.Cpf)) return Results.BadRequest("CPF inválido, use o formato 000.000.000-00");
            if(request.IdCondominio <= 0) return Results.BadRequest("Código de Condomínio inválido");
            if(request.QtdMoradores <= 0) return Results.BadRequest("Quantidade de moradores inválida");
            if(request.IdentificadorRes.Length < 3) return Results.BadRequest("Identificador de residência inválido");
            if(!UtilsService.IsValidEmail(request.Email)) return Results.BadRequest("Email inválido");
            if(!UtilsService.IsValidCpf(request.Cpf)) return Results.BadRequest("CPF inválido, use o formato 000.000.000-00");
            //check if HashSenha contain at least 8 characters, 1 uppercase, 1 lowercase, 1 number and 1 special character
            if(!UtilsService.IsValidPassword(request.Senha)) return Results.BadRequest("Senha inválida, deve conter no mínimo 8 caracteres, 1 letra maiúscula, 1 letra minúscula, 1 número e 1 caractere especial");
            
            var checkId = await _service.GetByIdAsync(id);
            if(checkId == null) return Results.NotFound("Morador não encontrado");
            
            
            var checkUserEmail = await _service.GetByEmailAsync(request.Email);
            
            if(checkUserEmail != null )
            {
                if (checkUserEmail.Auth.Email == request.Email && checkUserEmail.Id != id)
                {
                    return Results.BadRequest("Email já cadastrado");
                }
            }
                
            var checkUserCpf = await _service.GetByCpfAsync(request.Cpf);

            if(checkUserCpf != null)
            {
                if (checkUserCpf.Cpf == request.Cpf && checkUserCpf.Id != id)
                {
                    return Results.BadRequest("CPF já cadastrado");
                }
            }
            
            var checkCondominio = await _serviceCon.GetByIdAsync(request.IdCondominio);
            if(checkCondominio == null) return Results.BadRequest("Código de Condomínio inválido");

            var result = await _service.UpdateAsync(id, request);
            if(result == null) return Results.NotFound("Morador não encontrado");
            return Results.Ok(result);
        })
        .WithDescription("Atualiza um morador")
        .Produces<MoradorResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithName("UpdateMorador")
        .WithTags("Morador")
        .WithOpenApi(
            generatedOperation =>
            {
                generatedOperation.Parameters[0].Description = "Id do morador a ser atualizado";
                return generatedOperation;
            }
        );
        
        
        moradorGroup.MapDelete("/{id}", async (MoradorService _service, int id) =>
        {
            var result = await _service.DeleteAsync(id);
            return result != null ? Results.NoContent() : Results.NotFound("Morador não encontrado");
        })
        .WithDescription("Deleta um morador")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithName("DeleteMorador")
        .WithTags("Morador")
        .WithOpenApi(
            generatedOperation =>
            {
                generatedOperation.Parameters[0].Description = "Id do morador a ser deletado";
                return generatedOperation;
            }
        );
        
        moradorGroup.MapGet("email/{email}", async (MoradorService _service, string email) =>
        {
            var result = await _service.GetByEmailAsync(email);
            return result != null ? Results.Ok(result) : Results.NotFound("Morador não encontrado");
        })
        .WithDescription("Retorna um morador pelo email")
        .Produces<MoradorResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithName("GetMoradorByEmail")
        .WithTags("Morador")
        .WithOpenApi(
            generatedOperation =>
            {
                generatedOperation.Parameters[0].Description = "Email do morador a ser consultado";
                return generatedOperation;
            }
        );
    }
}