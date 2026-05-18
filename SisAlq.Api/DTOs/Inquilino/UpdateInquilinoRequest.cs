using System.ComponentModel.DataAnnotations;

namespace SisAlq.Api.DTOs.Inquilino;

public class UpdateInquilinoRequest
{
    [Required]
    [MaxLength(100)]
    public string RsocialNApellidos { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string CelularTelefono { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Direccion { get; set; } = string.Empty;

    [Required]
    [MaxLength(70)]
    [EmailAddress]
    public string Correo { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Referencia { get; set; }

    public bool Vigente { get; set; } = true;
}