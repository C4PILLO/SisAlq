using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.RecibosConsumo;

public static class GetReciboConsumoById
{
    public static async Task<IResult> Handle(int id, SisAlqDbContext db)
    {
        var recibo = await db.RecibosConsumo
            .Include(r => r.Inmueble)
            .Include(r => r.Inquilino)
            .Include(r => r.Moneda)
            .Include(r => r.Detalle)
                .ThenInclude(d => d.ConceptoConsumo)
            .FirstOrDefaultAsync(r => r.IdNroRecibo == id);

        if (recibo is null)
            return Results.NotFound(new { mensaje = "Recibo no encontrado." });

        return Results.Ok(recibo);
    }
}
