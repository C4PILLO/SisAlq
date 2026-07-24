using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Reportes;

public record ReporteContratoResponse(
    int IdContrato,
    string NroContrato,
    string Inquilino,
    List<string> Inmuebles,
    DateOnly FechaInicio,
    DateOnly FechaVcmto,
    string Estado
);

public static class GetReporteContratos
{
    public static async Task<IResult> Handle(string? estado, SisAlqDbContext db)
    {
        var datos = await ObtenerDatosAsync(estado, db);
        return Results.Ok(datos);
    }

    public static async Task<List<ReporteContratoResponse>> ObtenerDatosAsync(string? estado, SisAlqDbContext db)
    {
        var query = db.ContratosCab
            .Include(c => c.Inquilino)
            .Include(c => c.EstadoContrato)
            .Include(c => c.Detalle)
                .ThenInclude(d => d.Inmueble)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(estado))
        {
            var estadoNormalizado = estado.Trim().ToLower();
            query = estadoNormalizado switch
            {
                "vigente" => query.Where(c => c.EstadoContrato.Descripcion == "Vigente"),
                "vencido" => query.Where(c => c.EstadoContrato.Descripcion == "Vencido"),
                _ => query
            };
        }

        var contratos = await query
            .OrderByDescending(c => c.FechaVcmto)
            .ToListAsync();

        return contratos.Select(c => new ReporteContratoResponse(
            c.IdContrato,
            c.NroContrato,
            c.Inquilino.RsocialNApellidos,
            c.Detalle.Select(d => d.Inmueble.CodigoInmueble).ToList(),
            c.FechaInicio,
            c.FechaVcmto,
            c.EstadoContrato.Descripcion
        )).ToList();
    }
}