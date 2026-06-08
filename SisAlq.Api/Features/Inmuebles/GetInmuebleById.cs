using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Inmuebles;

public static class GetInmuebleById
{
    private static readonly int[] EstadosActivos = [1, 3];

    public static async Task<IResult> Handle(
        int id,
        SisAlqDbContext db,
        CancellationToken ct)
    {
        var inmueble = await db.Inmuebles
            .Include(i => i.TipoInmueble)
            .Include(i => i.Sector)
            .Include(i => i.EstadoInmueble)
            .Include(i => i.Moneda)
            .FirstOrDefaultAsync(i => i.IdInmueble == id, ct);

        if (inmueble is null)
            return Results.NotFound(new
            {
                status  = 404,
                error   = "Not Found",
                message = $"Inmueble con ID {id} no encontrado."
            });

        var detalle = await db.ContratosDetalle
            .Include(d => d.Contrato)
                .ThenInclude(c => c.Inquilino)
            .FirstOrDefaultAsync(d => d.IdInmueble == id
                                   && EstadosActivos.Contains(d.Contrato.IdEstadoContrato), ct);

        GetInmuebles.InquilinoResumen? resumen = null;
        if (detalle?.Contrato.Inquilino is { } inq)
        {
            resumen = new GetInmuebles.InquilinoResumen(
                inq.IdInquilino,
                inq.NroDocumento,
                inq.RsocialNApellidos,
                inq.CelularTelefono
            );
        }

        return Results.Ok(new GetInmuebles.Response(
            inmueble.IdInmueble,
            inmueble.CodigoInmueble,
            inmueble.DescripcionInmueble,
            inmueble.PisoInmueble,
            inmueble.PrecioAlquiler,
            inmueble.IncluyeServicios.ToString(),
            inmueble.Observaciones,
            inmueble.FechaRegistro,
            inmueble.TipoInmueble.Descripcion,
            inmueble.Sector.Descripcion,
            inmueble.EstadoInmueble.Descripcion,
            inmueble.Moneda.Descripcion,
            resumen
        ));
    }
}
