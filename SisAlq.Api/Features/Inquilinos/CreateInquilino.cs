using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;
using SisAlq.Api.Shared.Models;

namespace SisAlq.Api.Features.Inquilinos;

public static class CreateInquilino
{
    public record Request(
        int IdTipoCliente,
        int IdTDocumento,
        string NroDocumento,
        string RsocialNApellidos,
        string CelularTelefono,
        string Direccion,
        string Correo,
        string? Referencia
    );

    public static async Task<IResult> Handle(
        Request request,
        SisAlqDbContext db,
        CancellationToken ct)
    {
        // Validaciones
        if (string.IsNullOrWhiteSpace(request.NroDocumento))
            return Results.BadRequest(new { status = 400, error = "Bad Request", message = "El número de documento es requerido." });

        if (string.IsNullOrWhiteSpace(request.RsocialNApellidos))
            return Results.BadRequest(new { status = 400, error = "Bad Request", message = "El nombre o razón social es requerido." });

        if (string.IsNullOrWhiteSpace(request.Correo) || !request.Correo.Contains('@'))
            return Results.BadRequest(new { status = 400, error = "Bad Request", message = "El correo electrónico no es válido." });

        // Regla de negocio: DNI → Natural, RUC → Jurídica, CE → ambos
        var errorDocumento = (request.IdTipoCliente, request.IdTDocumento) switch
        {
            (1, 2) => "Una persona Natural no puede tener RUC.",
            (2, 1) => "Una persona Jurídica no puede tener DNI.",
            _ => null
        };

        if (errorDocumento is not null)
            return Results.BadRequest(new { status = 400, error = "Bad Request", message = errorDocumento });

        // Documento único
        if (await db.Inquilinos.AnyAsync(i => i.NroDocumento == request.NroDocumento.Trim(), ct))
            return Results.Conflict(new { status = 409, error = "Conflict", message = $"El documento '{request.NroDocumento}' ya está registrado." });

        var inquilino = new Inquilino
        {
            IdTipoCliente = request.IdTipoCliente,
            IdTDocumento = request.IdTDocumento,
            NroDocumento = request.NroDocumento.Trim(),
            RsocialNApellidos = request.RsocialNApellidos.Trim(),
            CelularTelefono = request.CelularTelefono.Trim(),
            Direccion = request.Direccion.Trim(),
            Correo = request.Correo.Trim().ToLower(),
            Referencia = request.Referencia?.Trim(),
            Vigente = true,
            FechaRegistro = DateTime.UtcNow
        };

        db.Inquilinos.Add(inquilino);
        await db.SaveChangesAsync(ct);

        await db.Entry(inquilino).Reference(i => i.TipoCliente).LoadAsync(ct);
        await db.Entry(inquilino).Reference(i => i.TipoDocumento).LoadAsync(ct);

        return Results.Created($"/api/inquilinos/{inquilino.IdInquilino}", new GetInquilinos.Response(
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