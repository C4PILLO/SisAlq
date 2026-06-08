using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Shared.Infrastructure;

public static class Seeder
{
    public static async Task SeedAdminUserAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SisAlqDbContext>();

        if (!db.Usuarios.Any())
        {
            db.Usuarios.Add(new SisAlq.Api.Shared.Models.Usuario
            {
                NombreUsuario = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                NombreApellidos = "Administrador SisAlq",
                IdRol = 1,
                Estado = true,
                FechaRegistro = DateTime.UtcNow
            });

            await db.SaveChangesAsync();
        }
    }
}
