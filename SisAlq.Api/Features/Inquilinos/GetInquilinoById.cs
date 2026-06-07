using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Inquilinos;

public static class GetInquilinoById
{
    public static async Task<IResult> Handle(
        int id,
        SisAlqDbContext db,
        CancellationToken ct)
    {
        var inquilino = await db.Inquilinos
            .Include(i => i.TipoCliente)
            .Include(i => i.TipoDocumento)
            .FirstOrDefaultAsync(i => i.IdInquilino == id, ct);

        if (inquilino is null)
            return Results.NotFound(new
            {
                status = 404,
                error = "Not Found",
                message = $"Inquilino con ID {id} no encontrado."
            });

        return Results.Ok(new GetInquilinos.Response(
            inquilino.IdInquilino,
            inquilino.NroDocumento,
            inquilino.RsocialNApellidos,
            inquilino.CelularTelefono,
            inquilino.Direccion,
            inquilino.Correo,
            inquilino.Referencia,
            inquilino.Vigente,
            inquilino.FechaRegistro,
            inquilino.TipoCliente.Descripcion,
            inquilino.TipoDocumento.Descripcion
        ));
    }
}