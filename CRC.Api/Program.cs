using CRC.Api.Models.Response;
using CRC.Api.Repository;
using CRC.Api.Resource;
using CRC.Api.Service;
using CRC.Api.Utils;
using CRC.Data;
using CRC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace CIDA.Api;

public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        #region Database

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddDbContext<CrcDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
            var serviceProvider = builder.Services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CrcDbContext>();

                context.Condominios.AddRange(
                    new Condominio
                    {
                        Id = 1,
                        Nome = "Condominio 1",
                        Endereco = "Rua 1"
                    }, new Condominio
                    {
                        Id = 2,
                        Nome = "Condominio 2",
                        Endereco = "Rua 2"
                    }, new Condominio
                    {
                        Id = 3,
                        Nome = "Condominio 3",
                        Endereco = "Rua 3"
                    });

                context.Auths.AddRange(
                    new Auth
                    {
                        Id = 1,
                        Email = "exemplo@exemplo.com",
                        HashSenha = UtilsService.QuickHash("123456"),
                    }, new Auth
                    {
                        Id = 2,
                        Email = "exemplo2@exemplo.com",
                        HashSenha = UtilsService.QuickHash("123456"),
                    }, new Auth
                    {
                        Id = 3,
                        Email = "exemplo3@exemplo.com",
                        HashSenha = UtilsService.QuickHash("123456"),
                    });

                context.Moradores.AddRange(
                    new Morador
                    {
                        Id = 1,
                        IdCondominio = 1,
                        IdAuth = 1,
                        Cpf = "123.456.789-01",
                        Nome = "Morador 1",
                        QtdMoradores = 1,
                        Pontos = 100,
                        IdentificadorRes = "Apto 1"
                    }, new Morador
                    {
                        Id = 2,
                        IdCondominio = 1,
                        IdAuth = 2,
                        Cpf = "123.456.789-02",
                        Nome = "Morador 2",
                        QtdMoradores = 2,
                        Pontos = 100,
                        IdentificadorRes = "Apto 2"
                    }, new Morador
                    {
                        Id = 3,
                        IdCondominio = 3,
                        IdAuth = 3,
                        Cpf = "123.456.789-03",
                        Nome = "Morador 3",
                        QtdMoradores = 3,
                        Pontos = 100,
                        IdentificadorRes = "Apto 3"
                    });

                context.Faturas.AddRange(
                    new Fatura
                    {
                        Id = 1,
                        IdMorador = 1,
                        QtdConsumida = 100,
                        DtGeracao = DateTime.Now
                    }, new Fatura
                    {
                        Id = 2,
                        IdMorador = 2,
                        QtdConsumida = 200,
                        DtGeracao = DateTime.Now
                    }, new Fatura
                    {
                        Id = 3,
                        IdMorador = 3,
                        QtdConsumida = 300,
                        DtGeracao = DateTime.Now
                    }, new Fatura
                    {
                        Id = 4,
                        IdMorador = 1,
                        QtdConsumida = 100,
                        DtGeracao = DateTime.Now - TimeSpan.FromDays(30)
                    });

                context.Bonus.AddRange(
                    new Bonus
                    {
                        Id = 1,
                        Nome = "Bonus 1",
                        Descricao = "Bonus 1",
                        QtdMax = 100,
                        Custo = 10,
                        IdCondominio = 1
                    }, new Bonus
                    {
                        Id = 2,
                        Nome = "Bonus 2",
                        Descricao = "Bonus 2",
                        QtdMax = 200,
                        Custo = 10,
                        IdCondominio = 2
                    }, new Bonus
                    {
                        Id = 3,
                        Nome = "Bonus 3",
                        Descricao = "Bonus 3",
                        QtdMax = 300,
                        Custo = 10,
                        IdCondominio = 3
                    });

                context.MoradorBonus.AddRange(
                    new MoradorBonus
                    {
                        Id = 1,
                        IdMorador = 1,
                        IdBonus = 1,
                        Qtd = 3
                    }, new MoradorBonus
                    {
                        Id = 2,
                        IdMorador = 2,
                        IdBonus = 2,
                        Qtd = 2
                    }, new MoradorBonus
                    {
                        Id = 3,
                        IdMorador = 3,
                        IdBonus = 3,
                        Qtd = 10
                    });
                
                context.SaveChanges();
            }
            

        }

        else
        {
            builder.Services.AddDbContext<CrcDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("AzureConnection"));
            });
        }

        #endregion

        #region Repositories_Services

        builder.Services.AddScoped<AuthRepository>();
        builder.Services.AddScoped<CondominioRepository>();
        builder.Services.AddScoped<MoradorRepository>();
        builder.Services.AddScoped<FaturaRepository>();
        builder.Services.AddScoped<BonusRepository>();
        builder.Services.AddScoped<MoradorBonusRepository>();

        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<CondominioService>();
        builder.Services.AddScoped<MoradorService>();
        builder.Services.AddScoped<FaturaService>();
        builder.Services.AddScoped<BonusService>();
        builder.Services.AddScoped<MoradorBonusService>();

        #endregion


        var app = builder.Build();


        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
        app.UseSwaggerUI();

        #region Rotas_alternativas_de_servico

        app.MapGet("/consumo/{idMorador:int}", async (CrcDbContext db, int idMorador) =>
            {
                var morador = await db.Moradores.FindAsync(idMorador);
                
                if (morador == null)
                {
                    return Results.NotFound("Morador não encontrado");
                }
                
                
                var faturaAtual = await db.Faturas
                    .Where(f => f.Morador.Id == idMorador)
                    .OrderByDescending(f => f.DtGeracao)
                    .FirstOrDefaultAsync();

                var faturaAnterior = await db.Faturas
                    .Where(f => f.Morador.Id == idMorador)
                    .Skip(1)
                    .OrderByDescending(f => f.DtGeracao)
                    .FirstOrDefaultAsync();

                if (faturaAtual == null || faturaAnterior == null)
                {
                    return Results.NotFound("Morador não possui faturas suficientes para calcular o consumo");
                }

                var porcentagemConsumo = (faturaAtual.QtdConsumida - faturaAnterior.QtdConsumida) /
                                         faturaAnterior.QtdConsumida;

                return Results.Ok(porcentagemConsumo);
            })
            .WithDescription("Calcula a porcentagem de consumo em relação à ultima fatura de luz do morador")
            .Produces<int>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags("Utils")
            .WithName("GetConsumo")
            .WithOpenApi(generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "Id do morador a qual o consumo será calculado";
                parameter.Required = true;
                return generatedOperation;
            });

        // Endpoint para gerar faturas aleatórias ( Não é um serviço real, apenas para testes no mobile )
        app.MapGet("/randomFatura/{idMorador:int}", async (CrcDbContext db, int idMorador) =>
            {
                var morador = await db.Moradores.FindAsync(idMorador);

                if (morador == null)
                {
                    return Results.NotFound("Morador não encontrado");
                }

                var fatura = new Fatura
                {
                    IdMorador = idMorador,
                    QtdConsumida = new Random().Next(70, 120),
                    DtGeracao = DateTime.Now - TimeSpan.FromDays(new Random().Next(1, 366))
                };

                decimal pontos = ((decimal)morador.QtdMoradores / fatura.QtdConsumida) * 1000;
                var pontosRedondo = (int)Math.Round(pontos);
                morador.Pontos += pontosRedondo;
                db.Moradores.Update(morador);

                await db.Faturas.AddAsync(fatura);
                await db.SaveChangesAsync();

                var response = new FaturaResponse(
                    fatura.Id,
                    fatura.IdMorador,
                    fatura.QtdConsumida,
                    fatura.DtGeracao
                );

                return Results.Ok(response);
            })
            .WithDescription("Cria uma fatura aleatória para um morador")
            .Produces<FaturaResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags("Utils")
            .WithName("CreateRandomFatura")
            .WithOpenApi(generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "Id do morador a qual a fatura será gerada";
                parameter.Required = true;
                return generatedOperation;
            });
        
        // Dar boot no banco de dados
        app.MapGet("/boot", async (CrcDbContext db) =>
            {
                int result = 0;

                try
                {
                    using (var connection = db.Database.GetDbConnection())
                    {
                        await connection.OpenAsync();
                        var command = connection.CreateCommand();
                        command.CommandText = "SELECT 1";

                        result = (int)await command.ExecuteScalarAsync();
                    }
                }
                catch (Exception e)
                {
                    result = 0;
                }

                return Results.Ok(result);
            })
            .WithName("Boot")
            .WithTags("Boot")
            .WithDescription("Endpoint para inicialização do banco de dados")
            .WithOpenApi();


        #endregion


        app.MapCondominioEndpoints();
        app.MapAuthEndpoints();
        app.MapMoradorEndpoints();
        app.MapFaturaEndpoints();
        app.MapBonusEndpoints();
        app.MapMoradorBonusEndpoints();
        app.MapPrevisaoEndpoints();

        app.Run();
    }
}
