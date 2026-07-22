namespace SisAlq.Api.Shared.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddCorsPolicy(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("SisAlqPolicy", policy =>
            {
                policy.WithOrigins(
                        "http://localhost:5173",
                        "https://gestion-pro-yw4g.vercel.app",
                        "http://localhost:5151",
                        "https://localhost:7235",
                        "http://localhost:5500",
                        "http://127.0.0.1:5500"
                      )
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        return services;
    }
}