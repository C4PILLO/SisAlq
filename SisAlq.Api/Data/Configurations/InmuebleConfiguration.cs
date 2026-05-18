using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAlq.Api.Models;

namespace SisAlq.Api.Data.Configurations;

public class InmuebleConfiguration : IEntityTypeConfiguration<Inmueble>
{
    public void Configure(EntityTypeBuilder<Inmueble> builder)
    {
        builder.ToTable("INMUEBLES");
        builder.HasKey(x => x.IdInmueble);

        builder.Property(x => x.CodigoInmueble)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(x => x.CodigoInmueble)
            .IsUnique();

        builder.Property(x => x.DescripcionInmueble)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.PisoInmueble)
            .HasMaxLength(10);

        builder.Property(x => x.PrecioAlquiler)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(x => x.IncluyeServicios)
            .IsRequired()
            .HasMaxLength(1);

        builder.Property(x => x.Observaciones)
            .HasMaxLength(250);

        builder.Property(x => x.FechaRegistro)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Relaciones
        builder.HasOne(x => x.TipoInmueble)
            .WithMany()
            .HasForeignKey(x => x.IdTipoInmueble);

        builder.HasOne(x => x.Sector)
            .WithMany()
            .HasForeignKey(x => x.IdSector);

        builder.HasOne(x => x.EstadoInmueble)
            .WithMany()
            .HasForeignKey(x => x.IdEstadoInmueble);

        builder.HasOne(x => x.Moneda)
            .WithMany()
            .HasForeignKey(x => x.IdMoneda);
    }
}