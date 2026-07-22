using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.ConceptosConsumo;

public record UpdateConceptoConsumoRequest(
    string Descripcion,
    string TipoConcepto,
    string? UnidadMedida,
    decimal Importe,
    bool Estado,
    bool Vigente
);

public static class UpdateConceptoConsumo
{
    private static readonly string[] TiposValidos = { "CONSUMO", "FIJO", "VARIABLE" };

    public static async Task<IResult> Handle(int id, UpdateConceptoConsumoRequest request, SisAlqDbContext db)
    {
        var concepto = await db.ConceptosConsumo.FirstOrDefaultAsync(c => c.IdConceptoConsumo == id);
        if (concepto is null)
            return Results.NotFound(new { mensaje = $"Concepto de consumo con ID {id} no encontrado." });

        if (string.IsNullOrWhiteSpace(request.Descripcion))
            return Results.BadRequest(new { mensaje = "Descripcion es obligatoria." });

        var tipoConcepto = request.TipoConcepto?.ToUpper().Trim() ?? string.Empty;
        if (!TiposValidos.Contains(tipoConcepto))
            return Results.BadRequest(new { mensaje = "TipoConcepto debe ser CONSUMO, FIJO o VARIABLE." });

        if (request.Importe < 0)
            return Results.BadRequest(new { mensaje = "Importe no puede ser negativo." });

        concepto.Descripcion = request.Descripcion.Trim();
        concepto.TipoConcepto = tipoConcepto;
        concepto.UnidadMedida = request.UnidadMedida?.Trim();
        concepto.Importe = request.Importe;
        concepto.Estado = request.Estado;
        concepto.Vigente = request.Vigente;

        await db.SaveChangesAsync();

        return Results.Ok(new
        {
            concepto.IdConceptoConsumo,
            concepto.DescCorta,
            concepto.Descripcion,
            concepto.TipoConcepto,
            concepto.UnidadMedida,
            concepto.Importe,
            concepto.Estado,
            concepto.Vigente,
            concepto.FechaCreacion
        });
    }
}
