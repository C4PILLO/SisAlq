namespace SisAlq.Api.Shared.Models;

public class ContratoDet
{
    public int IdContratoDet { get; set; }
    public int IdContrato { get; set; }
    public int IdInmueble { get; set; }
    public decimal RentaMensual { get; set; }
    public int NroMeses { get; set; }
    public int NroMesPPago { get; set; }

    // Navegación
    public ContratoCab Contrato { get; set; } = null!;
    public Inmueble Inmueble { get; set; } = null!;
}
