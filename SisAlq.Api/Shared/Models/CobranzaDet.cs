using SisAlq.Api.Shared.Models.Catalogos;

namespace SisAlq.Api.Shared.Models;

public class CobranzaDet
{
    public int IdCobranza { get; set; }
    public int Secuencia { get; set; }
    public string CodigoTD { get; set; } = string.Empty;
    public string NumeroDoc { get; set; } = string.Empty;
    public string CodigoMPago { get; set; } = string.Empty;
    public string? CodigoBanco { get; set; }
    public string? AliasBanco { get; set; }
    public string? NroOperacion { get; set; }
    public decimal Importe { get; set; }
    public decimal Mora { get; set; }
    public decimal Descuento { get; set; }
    public decimal TotalPagado { get; set; }
    public string EstadoPago { get; set; } = string.Empty;

    public CobranzaCab Cabecera { get; set; } = null!;
    public TipoComprobante TipoComprobante { get; set; } = null!;
    public MedioPago MedioPago { get; set; } = null!;
    public Banco? Banco { get; set; }
}
