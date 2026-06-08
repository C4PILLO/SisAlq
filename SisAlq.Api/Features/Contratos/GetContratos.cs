using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Contratos;

public static class GetContratos
{
    public record Response(
        int IdContrato,
        string NroContrato,
        string NombreInquilino,
        string CodigoInmueble,
        DateOnly FechaInicio,
        DateOnly FechaVcmto,
        int NroMeses,
        int NroMesPPago,
        decimal RentaMensual,
        decimal Garantia,
        string EstadoContrato,
        string Etiqueta
    );

    public static async Task<IResult> Handle(SisAlqDbContext db)
    {
        var hoy = DateOnly.FromDateTime(DateTime.UtcNow);

        var contratos = await db.ContratosCab
            .Include(c => c.Inquilino)
            .Include(c => c.EstadoContrato)
            .Include(c => c.Detalle)
                .ThenInclude(d => d.Inmueble)
            .OrderByDescending(c => c.FechaRegistro)
            .ToListAsync();

        var response = contratos.Select(c =>
        {
            var det     = c.Detalle.FirstOrDefault();
            var diasRestantes = c.FechaVcmto.DayNumber - hoy.DayNumber;

            var etiqueta = c.EstadoContrato.Descripcion switch
            {
                "Vencido"  => "VENCIDO",
                "Renovado" => "RENOVADO",
                _ => diasRestantes <= 30 ? "POR VENCER" : "VIGENTE"
            };

            return new Response(
                IdContrato:      c.IdContrato,
                NroContrato:     c.NroContrato,
                NombreInquilino: c.Inquilino.RsocialNApellidos,
                CodigoInmueble:  det?.Inmueble.CodigoInmueble ?? "-",
                FechaInicio:     c.FechaInicio,
                FechaVcmto:      c.FechaVcmto,
                NroMeses:        c.NroMeses,
                NroMesPPago:     det?.NroMesPPago ?? 0,
                RentaMensual:    det?.RentaMensual ?? 0,
                Garantia:        c.Garantia,
                EstadoContrato:  c.EstadoContrato.Descripcion,
                Etiqueta:        etiqueta
            );
        }).ToList();

        return Results.Ok(response);
    }
}
