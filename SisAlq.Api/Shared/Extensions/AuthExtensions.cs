using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace SisAlq.Api.Shared.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddJwtAuth(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                                                 Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var mensaje = context.AuthenticateFailure?.GetType().Name
                                      == "SecurityTokenExpiredException"
                            ? "El token ha expirado. Inicia sesión nuevamente."
                            : "No autorizado. Token inválido o ausente.";

                        await context.Response.WriteAsJsonAsync(new
                        {
                            status = 401,
                            error = "Unauthorized",
                            message = mensaje
                        });
                    },

                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";

                        await context.Response.WriteAsJsonAsync(new
                        {
                            status = 403,
                            error = "Forbidden",
                            message = "No tienes permisos para realizar esta acción."
                        });
                    }
                };
            });

        services.AddAuthorization();

        return services;
    }
}