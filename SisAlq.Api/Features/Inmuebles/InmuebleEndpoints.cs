namespace SisAlq.Api.Features.Inmuebles;

public static class InmuebleEndpoints
{
    public static void MapInmuebleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inmuebles")
                       .WithTags("Inmuebles")
                       .RequireAuthorization();

        group.MapGet("/", GetInmuebles.Handle)
             .WithSummary("Listar todos los inmuebles con estado de ocupación (HU-004)");

        group.MapGet("/{id:int}", GetInmuebleById.Handle)
             .WithSummary("Obtener inmueble por ID");

        group.MapPost("/", CreateInmueble.Handle)
             .WithSummary("Registrar nuevo inmueble (HU-001)")
             .RequireAuthorization(p => p.RequireRole("Administrador", "Asistente"));

        group.MapPut("/{id:int}", UpdateInmueble.Handle)
             .WithSummary("Actualizar inmueble")
             .RequireAuthorization(p => p.RequireRole("Administrador", "Asistente"));

        group.MapDelete("/{id:int}", DeleteInmueble.Handle)
             .WithSummary("Eliminar inmueble")
             .RequireAuthorization(p => p.RequireRole("Administrador"));
    }
}