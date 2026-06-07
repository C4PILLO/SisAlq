using SisAlq.Api.Shared.Models.Catalogos;

namespace SisAlq.Api.Shared.Models;

public class Inmueble
{
    public int IdInmueble { get; set; }
    public int IdTipoInmueble { get; set; }
    public int IdSector { get; set; }
    public int IdEstadoInmueble { get; set; }
    public int IdMoneda { get; set; }
    public string CodigoInmueble { get; set; } = string.Empty;
    public string DescripcionInmueble { get; set; } = string.Empty;
    public string? PisoInmueble { get; set; }
    public decimal PrecioAlquiler { get; set; }
    public char IncluyeServicios { get; set; }
    public string? Observaciones { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Navegación
    public TipoInmueble TipoInmueble { get; set; } = null!;
    public Sector Sector { get; set; } = null!;
    public EstadoInmueble EstadoInmueble { get; set; } = null!;
    public Moneda Moneda { get; set; } = null!;
}