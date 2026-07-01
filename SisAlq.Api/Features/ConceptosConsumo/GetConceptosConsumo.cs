using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.ConceptosConsumo;

public static class GetConceptosConsumo
{
    public static async Task<IResult> Handle(SisAlqDbContext db)
    {
        var conceptos = await db.ConceptosConsumo
            .Where(x => x.Vigente)
            .OrderBy(x => x.DescCorta)
            .Select(x => new
            {
                x.IdConceptoConsumo,
                x.DescCorta,
                x.Descripcion,
                x.TipoConcepto,
                x.UnidadMedida,
                x.Importe,
                x.Estado
            })
            .ToListAsync();

        return Results.Ok(conceptos);
    }
}
