using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.ConceptosConsumo;

public static class GetConceptoConsumoById
{
    public static async Task<IResult> Handle(int id, SisAlqDbContext db)
    {
        var concepto = await db.ConceptosConsumo
            .Where(c => c.IdConceptoConsumo == id)
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
            .FirstOrDefaultAsync();

        return concepto is null
            ? Results.NotFound(new { mensaje = $"Concepto de consumo con ID {id} no encontrado." })
            : Results.Ok(concepto);
    }
}
