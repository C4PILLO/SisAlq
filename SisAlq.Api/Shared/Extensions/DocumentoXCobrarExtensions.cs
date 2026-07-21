using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;
using SisAlq.Api.Shared.Models;

namespace SisAlq.Api.Shared.Extensions;

public static class DocumentoXCobrarExtensions
{
    public static async Task UpsertDocumentoXCobrarAsync(this SisAlqDbContext db, RingresoConsumoCab recibo)
    {
        var doc = await db.DocumentosXCobrar.FirstOrDefaultAsync(d =>
            d.IdInquilino == recibo.IdInquilino &&
            d.CodigoTD == "RI" &&
            d.NroDocumento == recibo.IdNroRecibo);

        if (doc is null)
        {
            db.DocumentosXCobrar.Add(new DocumentoXCobrar
            {
                IdInquilino  = recibo.IdInquilino,
                CodigoTD     = "RI",
                NroDocumento = recibo.IdNroRecibo,
                FechaEmision = recibo.FechaEmision,
                FechaVcmto   = recibo.FechaVencimiento,
                IdMoneda     = recibo.IdMoneda,
                Importe      = recibo.TotalRecibo,
                Saldo        = recibo.TotalRecibo,
                Usuario      = recibo.Usuario
            });
        }
        else
        {
            var yaCobrado = doc.Importe - doc.Saldo;
            doc.Importe = recibo.TotalRecibo;
            doc.Saldo   = recibo.TotalRecibo - yaCobrado;
        }
    }
}
