using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;
using SisAlq.Api.Shared.Models;

namespace SisAlq.Api.Features.RecibosConsumo;

public record CreateReciboConsumoRequest(int IdInmueble, string GlosaConcepto, string Usuario);

public static class CreateReciboConsumo
{
    public static async Task<IResult> Handle(CreateReciboConsumoRequest request, SisAlqDbContext db)
    {
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
                           r.GlosaConcepto == request.GlosaConcepto.ToUpper().Trim());

        if (existe)
            return Results.Conflict(new { mensaje = "Ya existe un recibo para ese periodo." });

        var recibo = new RingresoConsumoCab
        {
            IdInmueble       = request.IdInmueble,
            IdInquilino      = contrato.IdInquilino,
            IdMoneda         = contrato.IdMoneda,
            GlosaConcepto    = request.GlosaConcepto.ToUpper().Trim(),
            FechaEmision     = DateTime.UtcNow,
            FechaVencimiento = DateTime.UtcNow.AddDays(5),
            Usuario          = request.Usuario
        };

        db.RecibosConsumo.Add(recibo);
        await db.SaveChangesAsync();

        return Results.Created(
            string.Format("/api/recibos-consumo/{0}", recibo.IdNroRecibo),
            new { recibo.IdNroRecibo, recibo.GlosaConcepto, recibo.FechaEmision, recibo.FechaVencimiento, recibo.IdInquilino, recibo.IdMoneda, recibo.TotalRecibo });
    }
}
