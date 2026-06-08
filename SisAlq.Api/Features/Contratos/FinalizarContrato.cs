namespace SisAlq.Api.Features.Contratos;

public static class FinalizarContrato
{
    public static Task<IResult> Handle(int id)
        => Task.FromResult(Results.Ok("pendiente"));
}
