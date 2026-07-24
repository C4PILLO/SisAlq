using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;
using SisAlq.Api.Shared.Extensions;

namespace SisAlq.Api.Features.RecibosConsumo;

public static class DeleteDetalleRecibo
{
    public static async Task<IResult> Handle(int id, int item, SisAlqDbContext db)
    {
        var detalle = await db.RecibosConsumoDetalle
            .FirstOrDefaultAsync(d => d.IdNroRecibo == id && d.Item == item);

        if (detalle is null)
            return Results.NotFound(new { mensaje = "Linea de detalle no encontrada." });

        var recibo = await db.RecibosConsumo
            .Include(r => r.Detalle)
            .FirstAsync(r => r.IdNroRecibo == id);

        db.RecibosConsumoDetalle.Remove(detalle);
        recibo.TotalRecibo = recibo.Detalle.Where(d => d.Item != item).Sum(d => d.Importe);
        await db.UpsertDocumentoXCobrarAsync(recibo);
        await db.SaveChangesAsync();

        return Results.Ok(new { mensaje = "Concepto eliminado.", recibo.TotalRecibo });
    }
}

