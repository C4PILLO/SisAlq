namespace SisAlq.Api.Shared.Models;

public class ConceptoConsumoServicio
{
    public int IdConceptoConsumo { get; set; }
    public string DescCorta { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public string TipoConcepto { get; set; } = null!;
    public string? UnidadMedida { get; set; }
    public decimal Importe { get; set; } = 0;
    public bool Estado { get; set; } = true;
    public string? UsuarioCreacion { get; set; }
    public bool Vigente { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}
