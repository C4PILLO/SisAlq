namespace SisAlq.Api.Features.Reportes;

public static class ReporteEndpoints
{
    public static void MapReporteEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/reportes")
                       .WithTags("Reportes")
                       .RequireAuthorization();

        group.MapGet("/contratos", GetReporteContratos.Handle)
             .WithSummary("Reporte de contratos filtrable por estado (vigente/vencido)")
             .WithName("GetReporteContratos");
    }
}