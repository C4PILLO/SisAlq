using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Inmuebles;

public static class DeleteInmueble
{
    public static async Task<IResult> Handle(
        int id,
        SisAlqDbContext db,
        CancellationToken ct)
    {
        var inmueble = await db.Inmuebles.FindAsync([id], ct);

        if (inmueble is null)
            return Results.NotFound(new
            {
                status = 404,
                error = "Not Found",
                message = $"Inmueble con ID {id} no encontrado."
            });

        // TODO Sprint 2: bloquear si tiene contrato activo
        db.Inmuebles.Remove(inmueble);
        await db.SaveChangesAsync(ct);

        return Results.NoContent();
    }
}
