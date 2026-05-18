namespace SisAlq.Api.DTOs.Inmueble;

public class InmuebleResponse
{
    public int IdInmueble { get; set; }
    public string CodigoInmueble { get; set; } = string.Empty;
    public string DescripcionInmueble { get; set; } = string.Empty;
    public string? PisoInmueble { get; set; }
    public decimal PrecioAlquiler { get; set; }
    public string IncluyeServicios { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public DateTime FechaRegistro { get; set; }

    // Catálogos — se devuelve el texto, no el ID
    public string TipoInmueble { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public string EstadoInmueble { get; set; } = string.Empty;
    public string Moneda { get; set; } = string.Empty;

    // HU-004: inquilino actual (null si disponible)
    public InquilinoResumenDto? InquilinoActual { get; set; }
}

public class InquilinoResumenDto
{
    public int IdInquilino { get; set; }
    public string NroDocumento { get; set; } = string.Empty;
    public string RsocialNApellidos { get; set; } = string.Empty;
    public string Celular { get; set; } = string.Empty;
}