using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Inquilinos;

public static class UpdateInquilino
{
    public record Request(
        string RsocialNApellidos,
        string CelularTelefono,
        string Direccion,
        string Correo,
        string? Referencia,
        bool Vigente
    );

    public static async Task<IResult> Handle(
        int id,
        Request request,
        SisAlqDbContext db,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.RsocialNApellidos))
            return Results.BadRequest(new { status = 400, error = "Bad Request", message = "El nombre o razón social es requerido." });

        if (string.IsNullOrWhiteSpace(request.Correo) || !request.Correo.Contains('@'))
            return Results.BadRequest(new { status = 400, error = "Bad Request", message = "El correo electrónico no es válido." });

        var inquilino = await db.Inquilinos
            .Include(i => i.TipoCliente)
            .Include(i => i.TipoDocumento)
            .FirstOrDefaultAsync(i => i.IdInquilino == id, ct);

        if (inquilino is null)
            return Results.NotFound(new { status = 404, error = "Not Found", message = $"Inquilino con ID {id} no encontrado." });

        inquilino.RsocialNApellidos = request.RsocialNApellidos.Trim();
        inquilino.CelularTelefono = request.CelularTelefono.Trim();
        inquilino.Direccion = request.Direccion.Trim();
        inquilino.Correo = request.Correo.Trim().ToLower();
        inquilino.Referencia = request.Referencia?.Trim();
        inquilino.Vigente = request.Vigente;

        await db.SaveChangesAsync(ct);

        await db.Entry(inquilino).Reference(i => i.TipoCliente).LoadAsync(ct);
        await db.Entry(inquilino).Reference(i => i.TipoDocumento).LoadAsync(ct);

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