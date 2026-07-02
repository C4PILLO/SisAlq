using SisAlq.Api.Shared.Models.Catalogos;

namespace SisAlq.Api.Shared.Models;

public class ContratoCab
{
    public int IdContrato { get; set; }
    public string NroContrato { get; set; } = string.Empty;
    public DateOnly FechaContrato { get; set; }
    public int IdInquilino { get; set; }
    public string? Representante { get; set; }
    public string? TipoNegocio { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly FechaVcmto { get; set; }
    public int IdEstadoContrato { get; set; }
    public int NroMeses { get; set; }
    public int IdUsuario { get; set; }
    public int IdMoneda { get; set; }
    public decimal Garantia { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Navegación
    public Inquilino Inquilino { get; set; } = null!;
    public EstadoContrato EstadoContrato { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
    public Moneda Moneda { get; set; } = null!;
    public ICollection<ContratoDet> Detalle { get; set; } = [];
    public int MesesGarantia { get; set; }
    public string ModalidadPago { get; set; } = "Adelantado";
    public int CuotasPendientes { get; set; }
    public string? UrlDocumento { get; set; }
}
