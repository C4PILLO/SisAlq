namespace SisAlq.Api.Features.Auth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
                       .WithTags("Auth");

        group.MapPost("/login", Login.Handle)
             .WithSummary("Login — obtiene token JWT")
             .AllowAnonymous();
    }
}