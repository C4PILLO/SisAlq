using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Configuracion;

public static class GetParametroByClave
{
    public static async Task<IResult> Handle(string clave, SisAlqDbContext db)
    {
        var parametro = await db.Parametros.FirstOrDefaultAsync(p => p.Clave == clave.ToUpper());

        if (parametro is null)
            return Results.NotFound(new { mensaje = "Parametro no encontrado." });

        return Results.Ok(new { parametro.Clave, parametro.Valor, parametro.Descripcion });
    }
}
