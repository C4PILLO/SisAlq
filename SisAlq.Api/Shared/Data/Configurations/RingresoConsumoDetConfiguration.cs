using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAlq.Api.Shared.Models;

namespace SisAlq.Api.Shared.Data.Configurations;

public class RingresoConsumoDetConfiguration : IEntityTypeConfiguration<RingresoConsumoDet>
{
    public void Configure(EntityTypeBuilder<RingresoConsumoDet> builder)
    {
        builder.ToTable("RINGRESO_CONSUMODET");
        builder.HasKey(x => new { x.IdNroRecibo, x.Item });
        builder.Property(x => x.LecturaInicial).HasColumnType("decimal(12,2)");
        builder.Property(x => x.LecturaFinal).HasColumnType("decimal(12,2)");
        builder.Property(x => x.Importe).HasColumnType("decimal(10,2)").IsRequired();

        builder.HasOne(x => x.Cabecera).WithMany(c => c.Detalle).HasForeignKey(x => x.IdNroRecibo);
        builder.HasOne(x => x.ConceptoConsumo).WithMany().HasForeignKey(x => x.IdConceptoConsumo);
    }
}
