using SisAlq.Api.Shared.Models.Catalogos;

namespace SisAlq.Api.Shared.Models;

public class Inquilino
{
    public int IdInquilino { get; set; }
    public int IdTipoCliente { get; set; }
    public int IdTDocumento { get; set; }
    public string NroDocumento { get; set; } = string.Empty;
    public string RsocialNApellidos { get; set; } = string.Empty;
    public string CelularTelefono { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string? Referencia { get; set; }
    public bool Vigente { get; set; } = true;
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Navegación
    public TipoCliente TipoCliente { get; set; } = null!;
    public TipoDocumento TipoDocumento { get; set; } = null!;
}