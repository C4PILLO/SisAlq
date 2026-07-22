using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.ConceptosConsumo;

public static class GetConceptosConsumo
{
    public static async Task<IResult> Handle(SisAlqDbContext db)
    {
        var conceptos = await db.ConceptosConsumo
            .OrderBy(c => c.IdConceptoConsumo)
            .Select(c => new
            {
                c.IdConceptoConsumo,
                c.DescCorta,
                c.Descripcion,
                c.TipoConcepto,
                c.UnidadMedida,
                c.Importe,
                c.Estado,
                c.Vigente,
                c.FechaCreacion
            })
            .ToListAsync();

        return Results.Ok(conceptos);
    }
}
