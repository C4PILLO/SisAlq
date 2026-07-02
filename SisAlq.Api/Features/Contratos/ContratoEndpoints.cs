namespace SisAlq.Api.Features.Contratos;

public static class ContratoEndpoints
{
    public static void MapContratoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/contratos")
                       .WithTags("Contratos")
                       .RequireAuthorization();

        group.MapPost("/", CreateContrato.Handle)
             .WithSummary("Registrar nuevo contrato")
             .RequireAuthorization(p => p.RequireRole("Administrador", "Asistente"));

        group.MapGet("/", GetContratos.Handle)
             .WithSummary("Listar contratos con etiqueta de estado");

        group.MapGet("/{id:int}", GetContratoById.Handle)
             .WithSummary("Obtener contrato por ID");

        group.MapPut("/{id:int}/finalizar", FinalizarContrato.Handle)
             .WithSummary("Finalizar contrato y liberar inmueble")
             .RequireAuthorization(p => p.RequireRole("Administrador", "Asistente"));

        group.MapPut("/{id:int}/renovar", RenovarContrato.Handle)
             .WithSummary("Renovar contrato próximo a vencer")
             .RequireAuthorization(p => p.RequireRole("Administrador", "Asistente"));
        group.MapPut("/{id:int}/activar", ActivarContrato.Handle)
             .WithSummary("Activar contrato subiendo URL del documento PDF")
             .RequireAuthorization(p => p.RequireRole("Administrador", "Asistente"));
    }
}
