using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Contratos;

public static class GetContratoById
{
    public record Response(
        int IdContrato,
        string NroContrato,
        string NombreInquilino,
        string CodigoInmueble,
        string? Representante,
        string? TipoNegocio,
        DateOnly FechaContrato,
        DateOnly FechaInicio,
        DateOnly FechaVcmto,
        int NroMeses,
        int NroMesPPago,
        decimal RentaMensual,
        decimal Garantia,
        string Moneda,
        string EstadoContrato,
        string Etiqueta
    );

    public static async Task<IResult> Handle(int id, SisAlqDbContext db)
    {
        var hoy = DateOnly.FromDateTime(DateTime.UtcNow);

        var c = await db.ContratosCab
            .Include(c => c.Inquilino)
            .Include(c => c.EstadoContrato)
            .Include(c => c.Moneda)
            .Include(c => c.Detalle)
                .ThenInclude(d => d.Inmueble)
            .FirstOrDefaultAsync(c => c.IdContrato == id);

        if (c is null)
            return Results.NotFound(new { error = "Contrato no encontrado." });

        var det          = c.Detalle.FirstOrDefault();
        var diasRestantes = c.FechaVcmto.DayNumber - hoy.DayNumber;

        var etiqueta = c.EstadoContrato.Descripcion switch
        {
            "Vencido"  => "VENCIDO",
            "Renovado" => "RENOVADO",
            _ => diasRestantes <= 30 ? "POR VENCER" : "VIGENTE"
        };

        return Results.Ok(new Response(
            IdContrato:      c.IdContrato,
            NroContrato:     c.NroContrato,
            NombreInquilino: c.Inquilino.RsocialNApellidos,
            CodigoInmueble:  det?.Inmueble.CodigoInmueble ?? "-",
            Representante:   c.Representante,
            TipoNegocio:     c.TipoNegocio,
            FechaContrato:   c.FechaContrato,
            FechaInicio:     c.FechaInicio,
            FechaVcmto:      c.FechaVcmto,
            NroMeses:        c.NroMeses,
            NroMesPPago:     det?.NroMesPPago ?? 0,
            RentaMensual:    det?.RentaMensual ?? 0,
            Garantia:        c.Garantia,
            Moneda:          c.Moneda.Descripcion,
            EstadoContrato:  c.EstadoContrato.Descripcion,
            Etiqueta:        etiqueta
        ));
    }
}
