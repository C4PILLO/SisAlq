using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAlq.Api.Shared.Models;

namespace SisAlq.Api.Shared.Data.Configurations;

public class RingresoConsumoCabConfiguration : IEntityTypeConfiguration<RingresoConsumoCab>
{
    public void Configure(EntityTypeBuilder<RingresoConsumoCab> builder)
    {
        builder.ToTable("RINGRESO_CONSUMOCAB");
        builder.HasKey(x => x.IdNroRecibo);
        builder.Property(x => x.IdNroRecibo).ValueGeneratedOnAdd();
        builder.Property(x => x.GlosaConcepto).IsRequired().HasMaxLength(50);
        builder.Property(x => x.TotalRecibo).HasColumnType("decimal(12,2)").HasDefaultValue(0m);
        builder.Property(x => x.Usuario).IsRequired().HasMaxLength(15);
        builder.Property(x => x.TipoRecibo).IsRequired().HasMaxLength(20).HasDefaultValue("CONSUMO");
        builder.Property(x => x.FechaEmision).IsRequired().HasDefaultValueSql("NOW()");
        builder.Property(x => x.FechaVencimiento).IsRequired();
        builder.Property(x => x.FechaRegistro).IsRequired().HasDefaultValueSql("NOW()");

        builder.HasOne(x => x.Inmueble).WithMany().HasForeignKey(x => x.IdInmueble);
        builder.HasOne(x => x.Inquilino).WithMany().HasForeignKey(x => x.IdInquilino);
        builder.HasOne(x => x.Moneda).WithMany().HasForeignKey(x => x.IdMoneda);

        builder.HasIndex(x => new { x.IdInmueble, x.GlosaConcepto, x.TipoRecibo }).IsUnique();
    }
}
