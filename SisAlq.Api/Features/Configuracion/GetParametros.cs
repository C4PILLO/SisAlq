using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Configuracion;

public static class GetParametros
{
    public static async Task<IResult> Handle(SisAlqDbContext db)
    {
        var parametros = await db.Parametros
            .OrderBy(p => p.Clave)
            .Select(p => new
            {
                p.IdParametro,
                p.Clave,
                p.Valor,
                p.Descripcion
            })
            .ToListAsync();

        return Results.Ok(parametros);
    }
}
