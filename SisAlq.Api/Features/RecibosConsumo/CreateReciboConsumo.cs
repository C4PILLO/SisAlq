using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;
using SisAlq.Api.Shared.Models;

namespace SisAlq.Api.Features.RecibosConsumo;

public record CreateReciboConsumoRequest(
    int IdInmueble,
    string MesConsumo,
    string MesCobro,
    string Usuario
);

public static class CreateReciboConsumo
{
    public static async Task<IResult> Handle(CreateReciboConsumoRequest request, SisAlqDbContext db)
    {
        if (string.IsNullOrWhiteSpace(request.MesConsumo) || string.IsNullOrWhiteSpace(request.MesCobro))
            return Results.BadRequest(new { mensaje = "MesConsumo y MesCobro son obligatorios." });

        var glosa = string.Format("{0} / COBRO {1}",
            request.MesConsumo.ToUpper().Trim(),
            request.MesCobro.ToUpper().Trim());

        var contrato = await db.ContratosCab
            .Include(c => c.Inquilino)
            .Include(c => c.Moneda)
            .FirstOrDefaultAsync(c =>
                c.Detalle.Any(d => d.IdInmueble == request.IdInmueble) &&
                new[] { 1, 3 }.Contains(c.IdEstadoContrato));

        if (contrato is null)
            return Results.BadRequest(new { mensaje = "El inmueble no tiene un contrato vigente." });

        bool existe = await db.RecibosConsumo
            .AnyAsync(r => r.IdInmueble == request.IdInmueble &&
                           r.GlosaConcepto == glosa &&
                           r.TipoRecibo == "CONSUMO");

        if (existe)
            return Results.Conflict(new { mensaje = "Ya existe un recibo de consumo para ese periodo." });

        var diasParam = await db.Parametros
            .FirstOrDefaultAsync(p => p.Clave == "DIAS_VENCIMIENTO_RECIBO");

        int diasVencimiento = diasParam is not null ? int.Parse(diasParam.Valor) : 5;

        var recibo = new RingresoConsumoCab
        {
            IdInmueble = request.IdInmueble,
            IdInquilino = contrato.IdInquilino,
            IdMoneda = contrato.IdMoneda,
            GlosaConcepto = glosa,
            TipoRecibo = "CONSUMO",
            FechaEmision = DateTime.UtcNow,
            FechaVencimiento = DateTime.UtcNow.AddDays(diasVencimiento),
            Usuario = request.Usuario
        };

        db.RecibosConsumo.Add(recibo);
        await db.SaveChangesAsync();

        return Results.Created(
            string.Format("/api/recibos-consumo/{0}", recibo.IdNroRecibo),
            new
            {
                recibo.IdNroRecibo,
                recibo.GlosaConcepto,
                recibo.TipoRecibo,
                recibo.FechaEmision,
                recibo.FechaVencimiento,
                recibo.IdInquilino,
                recibo.IdMoneda,
                recibo.TotalRecibo
            });
    }
}