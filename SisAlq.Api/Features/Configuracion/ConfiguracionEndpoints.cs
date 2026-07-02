namespace SisAlq.Api.Features.Configuracion;

public static class ConfiguracionEndpoints
{
    public static void MapConfiguracionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/configuracion")
                       .WithTags("Configuracion")
                       .RequireAuthorization();

        group.MapGet("/parametros",       GetParametros.Handle);
        group.MapGet("/parametros/{clave}", GetParametroByClave.Handle);
        group.MapPut("/parametros/{clave}", UpdateParametro.Handle);
    }
}
