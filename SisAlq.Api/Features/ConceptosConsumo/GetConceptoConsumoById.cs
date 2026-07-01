using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.ConceptosConsumo;

public static class GetConceptoConsumoById
{
    public static async Task<IResult> Handle(int id, SisAlqDbContext db)
    {
        var concepto = await db.ConceptosConsumo.FindAsync(id);

        if (concepto is null || !concepto.Vigente)
            return Results.NotFound(new { mensaje = "Concepto no encontrado." });

        return Results.Ok(concepto);
    }
}
