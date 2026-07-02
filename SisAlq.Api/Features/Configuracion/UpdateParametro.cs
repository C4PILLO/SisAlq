using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Configuracion;

public record UpdateParametroRequest(string Valor);

public static class UpdateParametro
{
    public static async Task<IResult> Handle(string clave, UpdateParametroRequest request, SisAlqDbContext db)
    {
        var parametro = await db.Parametros.FirstOrDefaultAsync(p => p.Clave == clave.ToUpper());

        if (parametro is null)
            return Results.NotFound(new { mensaje = "Parametro no encontrado." });

        if (string.IsNullOrWhiteSpace(request.Valor))
            return Results.BadRequest(new { mensaje = "El valor no puede estar vacio." });

        parametro.Valor = request.Valor.Trim();
        await db.SaveChangesAsync();

        return Results.Ok(new { parametro.Clave, parametro.Valor, mensaje = "Parametro actualizado." });
    }
}
