namespace SisAlq.Api.Features.Contratos;

public static class GetContratos
{
    public static Task<IResult> Handle()
        => Task.FromResult(Results.Ok("pendiente"));
}
