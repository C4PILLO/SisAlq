using SisAlq.Api.Shared.Models.Catalogos;

namespace SisAlq.Api.Shared.Models;

public class CobranzaCab
{
    public int IdCobranza { get; set; }
    public DateTime FechaCobro { get; set; } = DateTime.UtcNow;
    public int IdInquilino { get; set; }
    public int IdMoneda { get; set; }
    public decimal TotalCobrado { get; set; }
    public decimal Mora { get; set; }
    public string? Observacion { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public string Estado { get; set; } = "A";

    public Inquilino Inquilino { get; set; } = null!;
    public Moneda Moneda { get; set; } = null!;
    public ICollection<CobranzaDet> Detalle { get; set; } = [];
}
