using System.ComponentModel.DataAnnotations;

namespace SisAlq.Api.DTOs.Inquilino;

public class CreateInquilinoRequest
{
    [Required]
    public int IdTipoCliente { get; set; }       // 1=Natural, 2=Juridica

    [Required]
    public int IdTDocumento { get; set; }         // 1=DNI, 2=RUC, 3=CE

    [Required]
    [MaxLength(20)]
    public string NroDocumento { get; set; } = string.Empty;

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
}