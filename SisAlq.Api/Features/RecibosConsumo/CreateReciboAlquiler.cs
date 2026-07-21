using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;
using SisAlq.Api.Shared.Extensions;
using SisAlq.Api.Shared.Models;

namespace SisAlq.Api.Features.RecibosConsumo;

public record CreateReciboAlquilerRequest(int IdContrato, string MesAlquiler, string Usuario);

public static class CreateReciboAlquiler
{
    public static async Task<IResult> Handle(CreateReciboAlquilerRequest request, SisAlqDbContext db)
    {
        if (string.IsNullOrWhiteSpace(request.MesAlquiler))
            return Results.BadRequest(new { mensaje = "MesAlquiler es obligatorio." });

        var contrato = await db.ContratosCab
            .Include(c => c.Detalle)
            .FirstOrDefaultAsync(c => c.IdContrato == request.IdContrato);

        if (contrato is null)
            return Results.NotFound(new { mensaje = "Contrato no encontrado." });

        if (contrato.IdEstadoContrato != 1 && contrato.IdEstadoContrato != 3)
            return Results.BadRequest(new { mensaje = "El contrato debe estar Vigente o Renovado." });

        if (!contrato.Detalle.Any())
            return Results.BadRequest(new { mensaje = "El contrato no tiene inmueble asignado." });

        if (contrato.CuotasPendientes <= 0)
            return Results.BadRequest(new { mensaje = "El contrato no tiene cuotas de alquiler pendientes." });

        var detalle = contrato.Detalle.First();
        var glosa = string.Format("ALQUILER {0}", request.MesAlquiler.ToUpper().Trim());

        bool existe = await db.RecibosConsumo.AnyAsync(r =>
            r.IdInmueble == detalle.IdInmueble &&
            r.GlosaConcepto == glosa &&
            r.TipoRecibo == "ALQUILER");

        if (existe)
            return Results.Conflict(new { mensaje = "Ya existe un recibo de alquiler para ese periodo." });

        var diasParam = await db.Parametros.FirstOrDefaultAsync(p => p.Clave == "DIAS_VENCIMIENTO_RECIBO");
        int dias = diasParam is not null ? int.Parse(diasParam.Valor) : 5;

        var recibo = new RingresoConsumoCab
        {
            IdInmueble       = detalle.IdInmueble,
            IdInquilino      = contrato.IdInquilino,
            IdMoneda         = contrato.IdMoneda,
            GlosaConcepto    = glosa,
            TipoRecibo       = "ALQUILER",
            FechaEmision     = DateTime.UtcNow,
            FechaVencimiento = DateTime.UtcNow.AddDays(dias),
            TotalRecibo      = detalle.RentaMensual,
            Usuario          = request.Usuario
        };

        db.RecibosConsumo.Add(recibo);
        await db.SaveChangesAsync();

        var det = new RingresoConsumoDet
        {
            IdNroRecibo       = recibo.IdNroRecibo,
            Item              = 1,
            IdConceptoConsumo = 9,
            Importe           = detalle.RentaMensual
        };
        db.RecibosConsumoDetalle.Add(det);

        contrato.CuotasPendientes -= 1;

        await db.UpsertDocumentoXCobrarAsync(recibo);
        await db.SaveChangesAsync();

        return Results.Created(
            string.Format("/api/recibos-consumo/{0}", recibo.IdNroRecibo),
            new
            {
                recibo.IdNroRecibo,
                recibo.GlosaConcepto,
                recibo.TotalRecibo,
                cuotasPendientes = contrato.CuotasPendientes
            });
    }
}
