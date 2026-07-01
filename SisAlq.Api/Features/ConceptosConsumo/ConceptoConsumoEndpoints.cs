namespace SisAlq.Api.Features.ConceptosConsumo;

public static class ConceptoConsumoEndpoints
{
    public static void MapConceptoConsumoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/conceptos-consumo")
                       .WithTags("Conceptos de Consumo")
                       .RequireAuthorization();

        group.MapGet("/",         GetConceptosConsumo.Handle);
        group.MapGet("/{id:int}", GetConceptoConsumoById.Handle);
        group.MapPost("/",        CreateConceptoConsumo.Handle);
        group.MapPut("/{id:int}", UpdateConceptoConsumo.Handle);
    }
}
