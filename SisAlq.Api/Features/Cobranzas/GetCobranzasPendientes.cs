using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Cobranzas;

public static class GetCobranzasPendientes
{
    public static async Task<IResult> Handle(SisAlqDbContext db, int? idInquilino, int? idInmueble)
    {
        var query = db.DocumentosXCobrar
            .Include(d => d.Inquilino)
            .Include(d => d.Moneda)
            .Where(d => d.Saldo > 0)
            .AsQueryable();

        if (idInquilino.HasValue)
            query = query.Where(d => d.IdInquilino == idInquilino.Value);

        var pendientes = await query.OrderBy(d => d.FechaVcmto).ToListAsync();

        if (idInmueble.HasValue)
        {
            var idsRecibosDelInmueble = await db.RecibosConsumo
                .Where(r => r.IdInmueble == idInmueble.Value)
                .Select(r => r.IdNroRecibo)
                .ToListAsync();

            pendientes = pendientes
                .Where(d => d.CodigoTD == "RI" && idsRecibosDelInmueble.Contains(d.NroDocumento))
                .ToList();
        }

        var resultado = pendientes.Select(d => new
        {
            d.IdInquilino,
            Inquilino = d.Inquilino.RsocialNApellidos,
            d.CodigoTD,
            d.NroDocumento,
            d.FechaEmision,
            d.FechaVcmto,
            Moneda = d.Moneda.Descripcion,
            d.Importe,
            d.Saldo
        });

        return Results.Ok(resultado);
    }
}
