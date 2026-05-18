using SisAlq.Api.DTOs.Auth;
using SisAlq.Api.Services;

namespace SisAlq.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/login", async (LoginRequest request, IAuthService authService) =>
        {
            if (string.IsNullOrWhiteSpace(request.Usuario) ||
                string.IsNullOrWhiteSpace(request.Contrasena))
                return Results.BadRequest("Usuario y contraseña son requeridos.");

            var response = await authService.LoginAsync(request);

            if (response is null)
                return Results.Unauthorized();

            return Results.Ok(response);
        })
        .WithSummary("Login — obtiene token JWT")
        .AllowAnonymous();
    }
}