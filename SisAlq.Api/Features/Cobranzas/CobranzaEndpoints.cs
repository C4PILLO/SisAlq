namespace SisAlq.Api.Features.Cobranzas;

public static class CobranzaEndpoints
{
    public static void MapCobranzaEndpoints(this IEndpointRouteBuilder app)
    {
        var catalogos = app.MapGroup("/api")
                           .WithTags("Catalogos Cobranza")
                           .RequireAuthorization();

        catalogos.MapGet("/bancos", GetBancos.Handle);
        catalogos.MapGet("/medios-pago", GetMediosPago.Handle);

        var group = app.MapGroup("/api/cobranzas")
                       .WithTags("Cobranzas")
                       .RequireAuthorization();

        group.MapGet("/pendientes", GetCobranzasPendientes.Handle);
        group.MapPost("/", CreateCobranza.Handle);
        group.MapPut("/{id:int}/anular", AnularCobranza.Handle);
    }
}
