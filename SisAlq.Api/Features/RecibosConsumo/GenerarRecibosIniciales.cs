using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;
using SisAlq.Api.Shared.Extensions;
using SisAlq.Api.Shared.Models;

namespace SisAlq.Api.Features.RecibosConsumo;

public record GenerarRecibosInicialesRequest(int IdContrato, string Usuario);

public static class GenerarRecibosIniciales
{
    public static async Task<IResult> Handle(GenerarRecibosInicialesRequest request, SisAlqDbContext db)
    {
        var contrato = await db.ContratosCab
            .Include(c => c.Detalle)
            .FirstOrDefaultAsync(c => c.IdContrato == request.IdContrato);

        if (contrato is null)
            return Results.NotFound(new { mensaje = "Contrato no encontrado." });

        if (contrato.IdEstadoContrato != 1 && contrato.IdEstadoContrato != 3)
            return Results.BadRequest(new { mensaje = "El contrato debe estar Vigente o Renovado." });

        if (!contrato.Detalle.Any())
            return Results.BadRequest(new { mensaje = "El contrato no tiene inmueble asignado." });

        var detalle = contrato.Detalle.First();

        var diasParam = await db.Parametros
            .FirstOrDefaultAsync(p => p.Clave == "DIAS_VENCIMIENTO_RECIBO");
        int dias = diasParam is not null ? int.Parse(diasParam.Valor) : 5;

        var recibosGenerados = new List<object>();

        // 1. Recibo de Garantia
        var montoGarantia = contrato.MesesGarantia * detalle.RentaMensual;
        var reciboGarantia = new RingresoConsumoCab
        {
            IdInmueble       = detalle.IdInmueble,
            IdInquilino      = contrato.IdInquilino,
            IdMoneda         = contrato.IdMoneda,
            GlosaConcepto    = "GARANTIA",
            TipoRecibo       = "ALQUILER",
            FechaEmision     = DateTime.UtcNow,
            FechaVencimiento = DateTime.UtcNow.AddDays(dias),
            TotalRecibo      = montoGarantia,
            Usuario          = request.Usuario
        };

        db.RecibosConsumo.Add(reciboGarantia);
        await db.SaveChangesAsync();
        await db.UpsertDocumentoXCobrarAsync(reciboGarantia);

        var detGarantia = new RingresoConsumoDet
        {
            IdNroRecibo       = reciboGarantia.IdNroRecibo,
            Item              = 1,
            IdConceptoConsumo = 10,
            Importe           = montoGarantia
        };
        db.RecibosConsumoDetalle.Add(detGarantia);

        recibosGenerados.Add(new
        {
            tipo = "GARANTIA",
            reciboGarantia.IdNroRecibo,
            reciboGarantia.GlosaConcepto,
            reciboGarantia.TotalRecibo
        });

        // 2. Recibo de Primer mes adelantado
        var reciboAlquiler = new RingresoConsumoCab
        {
            IdInmueble       = detalle.IdInmueble,
            IdInquilino      = contrato.IdInquilino,
            IdMoneda         = contrato.IdMoneda,
            GlosaConcepto    = "ALQUILER MES 1",
            TipoRecibo       = "ALQUILER",
            FechaEmision     = DateTime.UtcNow,
            FechaVencimiento = DateTime.UtcNow.AddDays(dias),
            TotalRecibo      = detalle.RentaMensual,
            Usuario          = request.Usuario
        };

        db.RecibosConsumo.Add(reciboAlquiler);
        await db.SaveChangesAsync();
        await db.UpsertDocumentoXCobrarAsync(reciboAlquiler);

        var detAlquiler = new RingresoConsumoDet
        {
            IdNroRecibo       = reciboAlquiler.IdNroRecibo,
            Item              = 1,
            IdConceptoConsumo = 9,
            Importe           = detalle.RentaMensual
        };
        db.RecibosConsumoDetalle.Add(detAlquiler);

        // 3. Descontar 1 cuota pendiente
        if (contrato.CuotasPendientes > 0)
            contrato.CuotasPendientes -= 1;

        await db.SaveChangesAsync();

        recibosGenerados.Add(new
        {
            tipo = "ALQUILER",
            reciboAlquiler.IdNroRecibo,
            reciboAlquiler.GlosaConcepto,
            reciboAlquiler.TotalRecibo
        });

        return Results.Ok(new
        {
            mensaje = "Recibos iniciales generados correctamente.",
            idContrato = request.IdContrato,
            cuotasPendientes = contrato.CuotasPendientes,
            recibos = recibosGenerados
        });
    }
}

