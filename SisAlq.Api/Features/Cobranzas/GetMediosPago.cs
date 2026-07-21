using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Cobranzas;

public static class GetMediosPago
{
    public static async Task<IResult> Handle(SisAlqDbContext db)
    {
        var medios = await db.MediosPago
            .OrderBy(m => m.CodigoMedioPago)
            .ToListAsync();

        return Results.Ok(medios);
    }
}
