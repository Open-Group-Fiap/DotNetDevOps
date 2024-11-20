using CRC.Api.Models.Request;
using CRC.Api.Models.Response;
using CRC.Api.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace CRC.Api.Resource;

public static class AuthResource
{
    
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var authGroup = app.MapGroup("/login");
        
        authGroup.MapPost("/", async (AuthRequest request, AuthService _service) =>
        {
            var result = await _service.Login(request);
            return result ? Results.Ok() : Results.Unauthorized();
        })
        .WithDescription("Login")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithName("Login")
        .WithTags("Login");
        
        
    }
}