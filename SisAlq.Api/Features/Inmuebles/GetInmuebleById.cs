using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Inmuebles;

public static class GetInmuebleById
{
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
                status = 404,
                error = "Not Found",
                message = $"Inmueble con ID {id} no encontrado."
            });

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
            null
        ));
    }
}