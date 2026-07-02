namespace SisAlq.Api.Shared.Models;

public class ConfiguracionParametro
{
    public int IdParametro { get; set; }
    public string Clave { get; set; } = null!;
    public string Valor { get; set; } = null!;
    public string? Descripcion { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}
