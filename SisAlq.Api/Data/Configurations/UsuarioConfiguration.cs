using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAlq.Api.Models;

namespace SisAlq.Api.Data.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("USUARIO");
        builder.HasKey(x => x.IdUsuario);

        builder.Property(x => x.NombreUsuario)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.NombreApellidos)
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(x => x.Correo)
            .HasMaxLength(70);

        builder.Property(x => x.CelularTelefono)
            .HasMaxLength(20);

        builder.Property(x => x.FechaRegistro)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Relación con Rol
        builder.HasOne(x => x.Rol)
            .WithMany()
            .HasForeignKey(x => x.IdRol);
    }
}