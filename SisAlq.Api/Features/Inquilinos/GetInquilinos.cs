using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Inquilinos;

public static class GetInquilinos
{
    public record Response(
        int IdInquilino,
        string NroDocumento,
        string RsocialNApellidos,
        string CelularTelefono,
        string Direccion,
        string Correo,
        string? Referencia,
        bool Vigente,
        DateTime FechaRegistro,
        string TipoCliente,
        string TipoDocumento
    );

    public static async Task<IResult> Handle(
        SisAlqDbContext db,
        CancellationToken ct)
    {
        var inquilinos = await db.Inquilinos
            .Include(i => i.TipoCliente)
            .Include(i => i.TipoDocumento)
            .OrderBy(i => i.RsocialNApellidos)
            .Select(i => new Response(
                i.IdInquilino,
                i.NroDocumento,
                i.RsocialNApellidos,
                i.CelularTelefono,
                i.Direccion,
                i.Correo,
                i.Referencia,
                i.Vigente,
                i.FechaRegistro,
                i.TipoCliente.Descripcion,
                i.TipoDocumento.Descripcion
            ))
            .ToListAsync(ct);

        return Results.Ok(inquilinos);
    }
}