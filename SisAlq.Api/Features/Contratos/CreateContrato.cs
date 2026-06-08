namespace SisAlq.Api.Features.Contratos;

public static class CreateContrato
{
    public static Task<IResult> Handle()
        => Task.FromResult(Results.Ok("pendiente"));
}
