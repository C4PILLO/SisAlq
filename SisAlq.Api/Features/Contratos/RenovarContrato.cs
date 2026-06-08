using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Contratos;

public static class RenovarContrato
{
    public record Request(int NroMesesRenovacion);

    public static async Task<IResult> Handle(
        int id,
        Request req,
        SisAlqDbContext db)
    {
        if (req.NroMesesRenovacion <= 0)
            return Results.BadRequest(new { error = "El número de meses de renovación debe ser mayor a 0." });

        var contrato = await db.ContratosCab
            .Include(c => c.EstadoContrato)
            .Include(c => c.Detalle)
            .FirstOrDefaultAsync(c => c.IdContrato == id);

        if (contrato is null)
            return Results.NotFound(new { error = "Contrato no encontrado." });

        if (contrato.EstadoContrato.Descripcion != "Vigente")
            return Results.BadRequest(new { error = $"Solo se pueden renovar contratos Vigentes. Estado actual: {contrato.EstadoContrato.Descripcion}." });

        // ── Extender fechas ───────────────────────────────────────
        var nuevaFechaVcmto = contrato.FechaVcmto.AddMonths(req.NroMesesRenovacion);
        contrato.FechaVcmto  = nuevaFechaVcmto;
        contrato.NroMeses   += req.NroMesesRenovacion;

        // ── Actualizar detalle ────────────────────────────────────
        foreach (var det in contrato.Detalle)
        {
            det.NroMeses    += req.NroMesesRenovacion;
            det.NroMesPPago  = req.NroMesesRenovacion; // cuotas pendientes de la renovación
        }

        // ── Cambiar estado → Renovado ─────────────────────────────
        contrato.IdEstadoContrato = 3; // Renovado

        await db.SaveChangesAsync();

        return Results.Ok(new
        {
            mensaje              = "Contrato renovado correctamente.",
            idContrato           = contrato.IdContrato,
            nroContrato          = contrato.NroContrato,
            nuevaFechaVcmto      = nuevaFechaVcmto,
            nroMesesTotal        = contrato.NroMeses,
            mesesRenovacion      = req.NroMesesRenovacion,
            estadoNuevo          = "Renovado"
        });
    }
}
