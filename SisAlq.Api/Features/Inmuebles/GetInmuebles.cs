using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Inmuebles;

public static class GetInmuebles
{
    public record Response(
        int IdInmueble,
        string CodigoInmueble,
        string DescripcionInmueble,
        string? PisoInmueble,
        decimal PrecioAlquiler,
        string IncluyeServicios,
        string? Observaciones,
        DateTime FechaRegistro,
        string TipoInmueble,
        string Sector,
        string EstadoInmueble,
        string Moneda,
        InquilinoResumen? InquilinoActual
    );

    public record InquilinoResumen(
        int IdInquilino,
        string NroDocumento,
        string RsocialNApellidos,
        string Celular
    );

    // Vigente = 1, Renovado = 3 — ambos implican inmueble ocupado
    private static readonly int[] EstadosActivos = [1, 3];

    public static async Task<IResult> Handle(
        SisAlqDbContext db,
        CancellationToken ct)
    {
        var inmuebles = await db.Inmuebles
            .Include(i => i.TipoInmueble)
            .Include(i => i.Sector)
            .Include(i => i.EstadoInmueble)
            .Include(i => i.Moneda)
            .ToListAsync(ct);

        var contratosActivos = await db.ContratosDetalle
            .Include(d => d.Contrato)
                .ThenInclude(c => c.Inquilino)
            .Where(d => EstadosActivos.Contains(d.Contrato.IdEstadoContrato))
            .ToListAsync(ct);

        var inquilinoPorInmueble = contratosActivos
            .GroupBy(d => d.IdInmueble)
            .ToDictionary(g => g.Key, g => g.First().Contrato.Inquilino);

        var response = inmuebles.Select(i =>
        {
            inquilinoPorInmueble.TryGetValue(i.IdInmueble, out var inq);
            var resumen = inq is null ? null : new InquilinoResumen(
                inq.IdInquilino,
                inq.NroDocumento,
                inq.RsocialNApellidos,
                inq.CelularTelefono
            );

            return new Response(
                i.IdInmueble,
                i.CodigoInmueble,
                i.DescripcionInmueble,
                i.PisoInmueble,
                i.PrecioAlquiler,
                i.IncluyeServicios.ToString(),
                i.Observaciones,
                i.FechaRegistro,
                i.TipoInmueble.Descripcion,
                i.Sector.Descripcion,
                i.EstadoInmueble.Descripcion,
                i.Moneda.Descripcion,
                resumen
            );
        }).ToList();

        return Results.Ok(response);
    }
}
