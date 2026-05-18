using System.ComponentModel.DataAnnotations;

namespace SisAlq.Api.DTOs.Inmueble;

public class CreateInmuebleRequest
{
    [Required]
    public int IdTipoInmueble { get; set; }

    [Required]
    public int IdSector { get; set; }

    [Required]
    public int IdEstadoInmueble { get; set; }

    [Required]
    public int IdMoneda { get; set; }

    [Required]
    [MaxLength(20)]
    public string CodigoInmueble { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string DescripcionInmueble { get; set; } = string.Empty;

    [MaxLength(10)]
    public string? PisoInmueble { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
    public decimal PrecioAlquiler { get; set; }

    [Required]
    [RegularExpression("^[SN]$", ErrorMessage = "IncluyeServicios debe ser 'S' o 'N'.")]
    public string IncluyeServicios { get; set; } = "N";

    [MaxLength(250)]
    public string? Observaciones { get; set; }
}