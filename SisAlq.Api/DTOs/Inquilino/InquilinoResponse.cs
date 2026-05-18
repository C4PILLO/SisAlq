namespace SisAlq.Api.DTOs.Inquilino;

public class InquilinoResponse
{
    public int IdInquilino { get; set; }
    public string NroDocumento { get; set; } = string.Empty;
    public string RsocialNApellidos { get; set; } = string.Empty;
    public string CelularTelefono { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string? Referencia { get; set; }
    public bool Vigente { get; set; }
    public DateTime FechaRegistro { get; set; }

    // Catálogos — texto, no ID
    public string TipoCliente { get; set; } = string.Empty;    // Natural / Juridica
    public string TipoDocumento { get; set; } = string.Empty;  // DNI / RUC / CE
}