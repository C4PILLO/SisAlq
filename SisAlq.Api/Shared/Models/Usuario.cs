using SisAlq.Api.Shared.Models.Catalogos;

namespace SisAlq.Api.Shared.Models;

public class Usuario
{
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string NombreApellidos { get; set; } = string.Empty;
    public int IdRol { get; set; }
    public string? Correo { get; set; }
    public string? CelularTelefono { get; set; }
    public bool Estado { get; set; } = true;
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Navegación
    public Rol Rol { get; set; } = null!;
}