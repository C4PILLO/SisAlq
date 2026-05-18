namespace SisAlq.Api.Models.Catalogos;

public class Rol
{
    public int IdRol { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public bool Estado { get; set; } = true;
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}