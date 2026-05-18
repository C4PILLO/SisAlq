using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAlq.Api.Models;

namespace SisAlq.Api.Data.Configurations;

public class InquilinoConfiguration : IEntityTypeConfiguration<Inquilino>
{
    public void Configure(EntityTypeBuilder<Inquilino> builder)
    {
        builder.ToTable("CLIENTE");
        builder.HasKey(x => x.IdInquilino);

        builder.Property(x => x.NroDocumento)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(x => x.NroDocumento)
            .IsUnique();

        builder.Property(x => x.RsocialNApellidos)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.CelularTelefono)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Direccion)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Correo)
            .IsRequired()
            .HasMaxLength(70);

        builder.Property(x => x.Referencia)
            .HasMaxLength(200);

        builder.Property(x => x.FechaRegistro)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Relaciones
        builder.HasOne(x => x.TipoCliente)
            .WithMany()
            .HasForeignKey(x => x.IdTipoCliente);

        builder.HasOne(x => x.TipoDocumento)
            .WithMany()
            .HasForeignKey(x => x.IdTDocumento);
    }
}