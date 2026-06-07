using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Inmuebles;

public static class UpdateInmueble
{
    public record Request(
        int IdTipoInmueble,
        int IdSector,
        int IdEstadoInmueble,
        int IdMoneda,
        string DescripcionInmueble,
        string? PisoInmueble,
        decimal PrecioAlquiler,
        string IncluyeServicios,
        string? Observaciones
    );

    public static async Task<IResult> Handle(
        int id,
        Request request,
        SisAlqDbContext db,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.DescripcionInmueble))
            return Results.BadRequest(new { status = 400, error = "Bad Request", message = "La descripción es requerida." });

        if (request.PrecioAlquiler <= 0)
            return Results.BadRequest(new { status = 400, error = "Bad Request", message = "El precio debe ser mayor a 0." });

        if (request.IncluyeServicios != "S" && request.IncluyeServicios != "N")
            return Results.BadRequest(new { status = 400, error = "Bad Request", message = "IncluyeServicios debe ser 'S' o 'N'." });

        var inmueble = await db.Inmuebles
            .Include(i => i.TipoInmueble)
            .Include(i => i.Sector)
            .Include(i => i.EstadoInmueble)
            .Include(i => i.Moneda)
            .FirstOrDefaultAsync(i => i.IdInmueble == id, ct);

        if (inmueble is null)
            return Results.NotFound(new { status = 404, error = "Not Found", message = $"Inmueble con ID {id} no encontrado." });

        inmueble.IdTipoInmueble = request.IdTipoInmueble;
        inmueble.IdSector = request.IdSector;
        inmueble.IdEstadoInmueble = request.IdEstadoInmueble;
        inmueble.IdMoneda = request.IdMoneda;
        inmueble.DescripcionInmueble = request.DescripcionInmueble.Trim();
        inmueble.PisoInmueble = request.PisoInmueble?.Trim();
        inmueble.PrecioAlquiler = request.PrecioAlquiler;
        inmueble.IncluyeServicios = request.IncluyeServicios[0];
        inmueble.Observaciones = request.Observaciones?.Trim();

        await db.SaveChangesAsync(ct);

        await db.Entry(inmueble).Reference(i => i.TipoInmueble).LoadAsync(ct);
        await db.Entry(inmueble).Reference(i => i.Sector).LoadAsync(ct);
        await db.Entry(inmueble).Reference(i => i.EstadoInmueble).LoadAsync(ct);
        await db.Entry(inmueble).Reference(i => i.Moneda).LoadAsync(ct);

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