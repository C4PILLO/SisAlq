using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Contratos;

public static class FinalizarContrato
{
    public static async Task<IResult> Handle(int id, SisAlqDbContext db)
    {
        var contrato = await db.ContratosCab
            .Include(c => c.EstadoContrato)
            .Include(c => c.Detalle)
                .ThenInclude(d => d.Inmueble)
            .FirstOrDefaultAsync(c => c.IdContrato == id);

        if (contrato is null)
            return Results.NotFound(new { error = "Contrato no encontrado." });

        if (contrato.EstadoContrato.Descripcion != "Vigente")
            return Results.BadRequest(new { error = $"Solo se pueden finalizar contratos Vigentes. Estado actual: {contrato.EstadoContrato.Descripcion}." });

        // ── Cambiar estado contrato → Vencido ────────────────────
        contrato.IdEstadoContrato = 2; // Vencido

        // ── Liberar inmueble → Disponible ────────────────────────
        var estadoDisponible = await db.EstadosInmueble
            .FirstAsync(e => e.Descripcion == "Disponible");

        foreach (var det in contrato.Detalle)
            det.Inmueble.IdEstadoInmueble = estadoDisponible.IdEstadoInmueble;

        await db.SaveChangesAsync();

        return Results.Ok(new
        {
            mensaje     = "Contrato finalizado correctamente.",
            idContrato  = contrato.IdContrato,
            nroContrato = contrato.NroContrato,
            estadoNuevo = "Vencido"
        });
    }
}
