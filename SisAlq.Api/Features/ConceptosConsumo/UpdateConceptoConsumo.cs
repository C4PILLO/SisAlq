using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.ConceptosConsumo;

public record UpdateConceptoConsumoRequest(
    string Descripcion,
    decimal Importe,
    string? UnidadMedida,
    bool Estado
);

public static class UpdateConceptoConsumo
{
    public static async Task<IResult> Handle(
        int id,
        UpdateConceptoConsumoRequest request,
        SisAlqDbContext db)
    {
        var concepto = await db.ConceptosConsumo.FindAsync(id);

        if (concepto is null || !concepto.Vigente)
            return Results.NotFound(new { mensaje = "Concepto no encontrado." });

        concepto.Descripcion  = request.Descripcion.Trim();
        concepto.Importe      = request.Importe;
        concepto.UnidadMedida = request.UnidadMedida?.Trim();
        concepto.Estado       = request.Estado;

        await db.SaveChangesAsync();

        return Results.Ok(concepto);
    }
}
