using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;
using SisAlq.Api.Shared.Extensions;
using SisAlq.Api.Shared.Models;

namespace SisAlq.Api.Features.RecibosConsumo;

public record AddDetalleReciboRequest(
    int IdConceptoConsumo,
    decimal? LecturaInicial,
    DateTime? FLecturaInicial,
    decimal? LecturaFinal,
    DateTime? FLecturaFinal,
    decimal? ImporteManual
);

public static class AddDetalleRecibo
{
    public static async Task<IResult> Handle(int id, AddDetalleReciboRequest request, SisAlqDbContext db)
    {
        var recibo = await db.RecibosConsumo
            .Include(r => r.Detalle)
            .FirstOrDefaultAsync(r => r.IdNroRecibo == id);

        if (recibo is null)
            return Results.NotFound(new { mensaje = "Recibo no encontrado." });

        var concepto = await db.ConceptosConsumo.FindAsync(request.IdConceptoConsumo);
        if (concepto is null || !concepto.Vigente)
            return Results.BadRequest(new { mensaje = "Concepto de consumo no valido." });

        decimal importe;
        switch (concepto.TipoConcepto)
        {
            case "CONSUMO":
                if (request.LecturaInicial is null || request.LecturaFinal is null)
                    return Results.BadRequest(new { mensaje = "Lecturas inicial y final son obligatorias para CONSUMO." });
                if (request.LecturaFinal <= request.LecturaInicial)
                    return Results.BadRequest(new { mensaje = "La lectura final debe ser mayor que la inicial." });
                importe = request.LecturaFinal.Value - request.LecturaInicial.Value;
                break;
            case "FIJO":
                importe = concepto.Importe;
                break;
            case "VARIABLE":
                if (request.ImporteManual is null || request.ImporteManual <= 0)
                    return Results.BadRequest(new { mensaje = "El importe es obligatorio para conceptos VARIABLE." });
                importe = request.ImporteManual.Value;
                break;
            default:
                return Results.BadRequest(new { mensaje = "TipoConcepto no reconocido." });
        }

        int nextItem = recibo.Detalle.Any() ? recibo.Detalle.Max(d => d.Item) + 1 : 1;

        var detalle = new RingresoConsumoDet
        {
            IdNroRecibo       = id,
            Item              = nextItem,
            IdConceptoConsumo = request.IdConceptoConsumo,
            LecturaInicial    = request.LecturaInicial,
            FLecturaInicial   = request.FLecturaInicial,
            LecturaFinal      = request.LecturaFinal,
            FLecturaFinal     = request.FLecturaFinal,
            Importe           = importe
        };

        db.RecibosConsumoDetalle.Add(detalle);
        //recibo.TotalRecibo = recibo.Detalle.Sum(d => d.Importe) + importe;
        recibo.TotalRecibo = recibo.Detalle
            .Where(d => d.Item != nextItem)
            .Sum(d => d.Importe) + importe;

        await db.UpsertDocumentoXCobrarAsync(recibo);
        await db.SaveChangesAsync();

        return Results.Ok(new { mensaje = "Concepto agregado.", detalle.Item, detalle.Importe, recibo.TotalRecibo });
    }
}

