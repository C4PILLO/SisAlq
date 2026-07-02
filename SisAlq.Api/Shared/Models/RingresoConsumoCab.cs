using SisAlq.Api.Shared.Models.Catalogos;

namespace SisAlq.Api.Shared.Models;

public class RingresoConsumoCab
{
    public int IdNroRecibo { get; set; }
    public int IdInmueble { get; set; }
    public int IdInquilino { get; set; }
    public int IdMoneda { get; set; }
    public DateTime FechaEmision { get; set; } = DateTime.UtcNow;
    public DateTime FechaVencimiento { get; set; }
    public string GlosaConcepto { get; set; } = null!;
    public decimal TotalRecibo { get; set; } = 0;
    public string Usuario { get; set; } = null!;
    public string TipoRecibo { get; set; } = "CONSUMO"; // CONSUMO | ALQUILER
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    public Inmueble Inmueble { get; set; } = null!;
    public Inquilino Inquilino { get; set; } = null!;
    public Moneda Moneda { get; set; } = null!;
    public ICollection<RingresoConsumoDet> Detalle { get; set; } = [];
}
