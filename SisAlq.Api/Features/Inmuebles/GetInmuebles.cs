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

    public static async Task<IResult> Handle(
        SisAlqDbContext db,
        CancellationToken ct)
    {
        var inmuebles = await db.Inmuebles
            .Include(i => i.TipoInmueble)
            .Include(i => i.Sector)
            .Include(i => i.EstadoInmueble)
            .Include(i => i.Moneda)
            .Select(i => new Response(
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
                null // InquilinoActual — se completa en Sprint 2 con contratos
            ))
            .ToListAsync(ct);

        return Results.Ok(inmuebles);
    }
}