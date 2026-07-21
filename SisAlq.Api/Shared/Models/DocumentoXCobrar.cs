using SisAlq.Api.Shared.Models.Catalogos;

namespace SisAlq.Api.Shared.Models;

public class DocumentoXCobrar
{
    public int IdInquilino { get; set; }
    public string CodigoTD { get; set; } = "RI";
    public int NroDocumento { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime FechaVcmto { get; set; }
    public int IdMoneda { get; set; }
    public decimal Importe { get; set; }
    public decimal Saldo { get; set; }
    public string Usuario { get; set; } = string.Empty;

    public Inquilino Inquilino { get; set; } = null!;
    public TipoComprobante TipoComprobante { get; set; } = null!;
    public Moneda Moneda { get; set; } = null!;
}
