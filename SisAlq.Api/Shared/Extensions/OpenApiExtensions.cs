using Scalar.AspNetCore;

namespace SisAlq.Api.Shared.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApiDocs(
        this IServiceCollection services)
    {
        services.AddOpenApi();
        return services;
    }

    public static WebApplication UseOpenApiDocs(
        this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.WithTitle("SisAlq API — MAEL S.R.L.")
                   .WithTheme(ScalarTheme.DeepSpace)
                   .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });

        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        return app;
    }
}
