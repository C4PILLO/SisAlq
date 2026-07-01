using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.RecibosConsumo;

public static class GetRecibosConsumo
{
    public static async Task<IResult> Handle(SisAlqDbContext db)
    {
        var recibos = await db.RecibosConsumo
            .Include(r => r.Inmueble)
            .Include(r => r.Inquilino)
            .Include(r => r.Moneda)
            .OrderByDescending(r => r.FechaEmision)
            .Select(r => new
            {
                r.IdNroRecibo,
                r.GlosaConcepto,
                r.FechaEmision,
                r.FechaVencimiento,
                r.TotalRecibo,
                r.Usuario,
                Inmueble  = r.Inmueble.CodigoInmueble,
                Inquilino = r.Inquilino.RsocialNApellidos,
                Moneda    = r.Moneda.Descripcion
            })
            .ToListAsync();

        return Results.Ok(recibos);
    }
}
