using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;
using SisAlq.Api.Shared.Models;

namespace SisAlq.Api.Features.ConceptosConsumo;

public record CreateConceptoConsumoRequest(
    string DescCorta,
    string Descripcion,
    string TipoConcepto,
    string? UnidadMedida,
    decimal Importe,
    string? UsuarioCreacion
);

public static class CreateConceptoConsumo
{
    private static readonly string[] TiposValidos = ["FIJO", "VARIABLE", "CONSUMO"];

    public static async Task<IResult> Handle(
        CreateConceptoConsumoRequest request,
        SisAlqDbContext db)
    {
        if (string.IsNullOrWhiteSpace(request.DescCorta))
            return Results.BadRequest(new { mensaje = "La descripción corta es obligatoria." });

        if (!TiposValidos.Contains(request.TipoConcepto.ToUpper()))
            return Results.BadRequest(new { mensaje = "TipoConcepto debe ser FIJO, VARIABLE o CONSUMO." });

        bool existe = await db.ConceptosConsumo
            .AnyAsync(x => x.DescCorta.ToUpper() == request.DescCorta.ToUpper() && x.Vigente);

        if (existe)
            return Results.Conflict(new { mensaje = "Ya existe un concepto con esa descripción corta." });

        var concepto = new ConceptoConsumoServicio
        {
            DescCorta       = request.DescCorta.ToUpper().Trim(),
            Descripcion     = request.Descripcion.Trim(),
            TipoConcepto    = request.TipoConcepto.ToUpper(),
            UnidadMedida    = request.UnidadMedida?.Trim(),
            Importe         = request.Importe,
            UsuarioCreacion = request.UsuarioCreacion
        };

        db.ConceptosConsumo.Add(concepto);
        await db.SaveChangesAsync();

        return Results.Created($"/api/conceptos-consumo/{concepto.IdConceptoConsumo}", concepto);
    }
}
