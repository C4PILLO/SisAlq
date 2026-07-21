using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Cobranzas;

public static class GetBancos
{
    public static async Task<IResult> Handle(SisAlqDbContext db)
    {
        var bancos = await db.Bancos
            .Where(b => b.Estado == "A")
            .OrderBy(b => b.CodigoBanco)
            .ToListAsync();

        return Results.Ok(bancos);
    }
}
