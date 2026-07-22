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
    private static readonly string[] TiposValidos = { "CONSUMO", "FIJO", "VARIABLE" };

    public static async Task<IResult> Handle(CreateConceptoConsumoRequest request, SisAlqDbContext db)
    {
        if (string.IsNullOrWhiteSpace(request.DescCorta))
            return Results.BadRequest(new { mensaje = "DescCorta es obligatorio." });

        if (string.IsNullOrWhiteSpace(request.Descripcion))
            return Results.BadRequest(new { mensaje = "Descripcion es obligatoria." });

        var tipoConcepto = request.TipoConcepto?.ToUpper().Trim() ?? string.Empty;
        if (!TiposValidos.Contains(tipoConcepto))
            return Results.BadRequest(new { mensaje = "TipoConcepto debe ser CONSUMO, FIJO o VARIABLE." });

        if (request.Importe < 0)
            return Results.BadRequest(new { mensaje = "Importe no puede ser negativo." });

        var descCortaNormalizada = request.DescCorta.Trim().ToUpper();

        bool existeDescCorta = await db.ConceptosConsumo
            .AnyAsync(c => c.DescCorta == descCortaNormalizada);

        if (existeDescCorta)
            return Results.Conflict(new { mensaje = $"Ya existe un concepto con DescCorta '{descCortaNormalizada}'." });

        var concepto = new ConceptoConsumoServicio
        {
            DescCorta = descCortaNormalizada,
            Descripcion = request.Descripcion.Trim(),
            TipoConcepto = tipoConcepto,
            UnidadMedida = request.UnidadMedida?.Trim(),
            Importe = request.Importe,
            Estado = true,
            Vigente = true,
            UsuarioCreacion = request.UsuarioCreacion,
            FechaCreacion = DateTime.UtcNow,
            FechaRegistro = DateTime.UtcNow
        };

        db.ConceptosConsumo.Add(concepto);
        await db.SaveChangesAsync();

        return Results.Created(
            $"/api/conceptos-consumo/{concepto.IdConceptoConsumo}",
            new
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
