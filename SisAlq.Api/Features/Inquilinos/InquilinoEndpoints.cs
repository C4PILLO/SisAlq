namespace SisAlq.Api.Features.Inquilinos;

public static class InquilinoEndpoints
{
    public static void MapInquilinoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inquilinos")
                       .WithTags("Inquilinos")
                       .RequireAuthorization();

        group.MapGet("/", GetInquilinos.Handle)
             .WithSummary("Listar todos los inquilinos");

        group.MapGet("/{id:int}", GetInquilinoById.Handle)
             .WithSummary("Obtener inquilino por ID");

        group.MapPost("/", CreateInquilino.Handle)
             .WithSummary("Registrar nuevo inquilino (HU-002)")
             .RequireAuthorization(p => p.RequireRole("Administrador", "Asistente"));

        group.MapPut("/{id:int}", UpdateInquilino.Handle)
             .WithSummary("Actualizar datos del inquilino")
             .RequireAuthorization(p => p.RequireRole("Administrador", "Asistente"));
    }
}