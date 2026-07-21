using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Cobranzas;

public record AnularCobranzaRequest(string Usuario, string? Motivo);

public static class AnularCobranza
{
    public static async Task<IResult> Handle(int id, AnularCobranzaRequest request, SisAlqDbContext db)
    {
        var cobranza = await db.CobranzasCab
            .Include(c => c.Detalle)
            .FirstOrDefaultAsync(c => c.IdCobranza == id);

        if (cobranza is null)
            return Results.NotFound(new { mensaje = "Cobranza no encontrada." });

        if (cobranza.Estado == "N")
            return Results.Conflict(new { mensaje = "La cobranza ya esta anulada." });

        var strategy = db.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await db.Database.BeginTransactionAsync();

            foreach (var det in cobranza.Detalle)
            {
                if (!int.TryParse(det.NumeroDoc, out var nroDocumento))
                    continue;

                var doc = await db.DocumentosXCobrar.FirstOrDefaultAsync(d =>
                    d.IdInquilino == cobranza.IdInquilino &&
                    d.CodigoTD == det.CodigoTD &&
                    d.NroDocumento == nroDocumento);

                if (doc is not null)
                {
                    doc.Saldo += det.TotalPagado;
                    det.EstadoPago = "Anulado";
                }
            }

            cobranza.Estado = "N";
            cobranza.Observacion = string.IsNullOrWhiteSpace(request.Motivo)
                ? cobranza.Observacion
                : string.Format("{0} | ANULADO ({1}): {2}", cobranza.Observacion, request.Usuario, request.Motivo);

            await db.SaveChangesAsync();
            await transaction.CommitAsync();
        });

        return Results.Ok(new { mensaje = "Cobranza anulada correctamente.", cobranza.IdCobranza, cobranza.Estado });
    }
}
