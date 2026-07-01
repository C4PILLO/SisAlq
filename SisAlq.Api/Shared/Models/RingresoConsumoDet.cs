namespace SisAlq.Api.Shared.Models;

public class RingresoConsumoDet
{
    public int IdNroRecibo { get; set; }
    public int Item { get; set; }
    public int IdConceptoConsumo { get; set; }
    public decimal? LecturaInicial { get; set; }
    public DateTime? FLecturaInicial { get; set; }
    public decimal? LecturaFinal { get; set; }
    public DateTime? FLecturaFinal { get; set; }
    public decimal Importe { get; set; }

    public RingresoConsumoCab Cabecera { get; set; } = null!;
    public ConceptoConsumoServicio ConceptoConsumo { get; set; } = null!;
}
