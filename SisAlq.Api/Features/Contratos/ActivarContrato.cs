using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Contratos;

public record ActivarContratoRequest(string UrlDocumento);

public static class ActivarContrato
{
    public static async Task<IResult> Handle(int id, ActivarContratoRequest request, SisAlqDbContext db)
    {
        if (string.IsNullOrWhiteSpace(request.UrlDocumento))
            return Results.BadRequest(new { error = "La URL del documento es obligatoria." });

        var contrato = await db.ContratosCab
            .Include(c => c.Detalle)
            .FirstOrDefaultAsync(c => c.IdContrato == id);

        if (contrato is null)
            return Results.NotFound(new { error = "Contrato no encontrado." });

        if (contrato.IdEstadoContrato != 4)
            return Results.BadRequest(new { error = "El contrato no esta en estado Doc Pendiente." });

        var inmueble = await db.Inmuebles
            .Include(i => i.EstadoInmueble)
            .FirstOrDefaultAsync(i => i.IdInmueble == contrato.Detalle.First().IdInmueble);

        if (inmueble is null)
            return Results.BadRequest(new { error = "Inmueble del contrato no encontrado." });

        var estadoOcupado = await db.EstadosInmueble
            .FirstAsync(e => e.Descripcion == "Ocupado");

        contrato.IdEstadoContrato = 1; // Vigente
        contrato.UrlDocumento = request.UrlDocumento.Trim();
        inmueble.IdEstadoInmueble = estadoOcupado.IdEstadoInmueble;

        await db.SaveChangesAsync();

        return Results.Ok(new
        {
            contrato.IdContrato,
            contrato.NroContrato,
            contrato.UrlDocumento,
            EstadoContrato = "Vigente",
            InmuebleEstado = "Ocupado",
            mensaje = "Contrato activado correctamente."
        });
    }
}
