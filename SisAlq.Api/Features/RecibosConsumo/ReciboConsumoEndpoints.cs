namespace SisAlq.Api.Features.RecibosConsumo;

public static class ReciboConsumoEndpoints
{
    public static void MapReciboConsumoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/recibos-consumo")
                       .WithTags("Recibos de Consumo")
                       .RequireAuthorization();

        group.MapGet("/",                               GetRecibosConsumo.Handle);
        group.MapGet("/{id:int}",                       GetReciboConsumoById.Handle);
        group.MapPost("/",                              CreateReciboConsumo.Handle);
        group.MapPost("/{id:int}/detalle",              AddDetalleRecibo.Handle);
        group.MapDelete("/{id:int}/detalle/{item:int}", DeleteDetalleRecibo.Handle);
    }
}
