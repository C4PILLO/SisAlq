using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;
using SisAlq.Api.Shared.Models;

namespace SisAlq.Api.Features.Inmuebles;

public static class CreateInmueble
{
    public record Request(
        int IdTipoInmueble,
        int IdSector,
        int IdEstadoInmueble,
        int IdMoneda,
        string CodigoInmueble,
        string DescripcionInmueble,
        string? PisoInmueble,
        decimal PrecioAlquiler,
        string IncluyeServicios,
        string? Observaciones
    );

    public static async Task<IResult> Handle(
        Request request,
        SisAlqDbContext db,
        CancellationToken ct)
    {
        // Validaciones
        if (string.IsNullOrWhiteSpace(request.CodigoInmueble))
            return Results.BadRequest(new { status = 400, error = "Bad Request", message = "El código del inmueble es requerido." });

        if (string.IsNullOrWhiteSpace(request.DescripcionInmueble))
            return Results.BadRequest(new { status = 400, error = "Bad Request", message = "La descripción es requerida." });

        if (request.PrecioAlquiler <= 0)
            return Results.BadRequest(new { status = 400, error = "Bad Request", message = "El precio debe ser mayor a 0." });

        if (request.IncluyeServicios != "S" && request.IncluyeServicios != "N")
            return Results.BadRequest(new { status = 400, error = "Bad Request", message = "IncluyeServicios debe ser 'S' o 'N'." });

        // Código único
        var codigoNormalizado = request.CodigoInmueble.Trim().ToUpper();
        if (await db.Inmuebles.AnyAsync(i => i.CodigoInmueble == codigoNormalizado, ct))
            return Results.Conflict(new { status = 409, error = "Conflict", message = $"El código '{codigoNormalizado}' ya está registrado." });

        var inmueble = new Inmueble
        {
            IdTipoInmueble = request.IdTipoInmueble,
            IdSector = request.IdSector,
            IdEstadoInmueble = request.IdEstadoInmueble,
            IdMoneda = request.IdMoneda,
            CodigoInmueble = codigoNormalizado,
            DescripcionInmueble = request.DescripcionInmueble.Trim(),
            PisoInmueble = request.PisoInmueble?.Trim(),
            PrecioAlquiler = request.PrecioAlquiler,
            IncluyeServicios = request.IncluyeServicios[0],
            Observaciones = request.Observaciones?.Trim(),
            FechaRegistro = DateTime.UtcNow
        };

        db.Inmuebles.Add(inmueble);
        await db.SaveChangesAsync(ct);

        await db.Entry(inmueble).Reference(i => i.TipoInmueble).LoadAsync(ct);
        await db.Entry(inmueble).Reference(i => i.Sector).LoadAsync(ct);
        await db.Entry(inmueble).Reference(i => i.EstadoInmueble).LoadAsync(ct);
        await db.Entry(inmueble).Reference(i => i.Moneda).LoadAsync(ct);

        return Results.Created($"/api/inmuebles/{inmueble.IdInmueble}", new GetInmuebles.Response(
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