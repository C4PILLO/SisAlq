using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAlq.Api.Shared.Models;

namespace SisAlq.Api.Shared.Data.Configurations;

public class ConceptoConsumoServicioConfiguration : IEntityTypeConfiguration<ConceptoConsumoServicio>
{
    public void Configure(EntityTypeBuilder<ConceptoConsumoServicio> builder)
    {
        builder.ToTable("CONCEPTO_CONSUMOSERVICIO");
        builder.HasKey(x => x.IdConceptoConsumo);
        builder.Property(x => x.DescCorta).IsRequired().HasMaxLength(20);
        builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(100);
        builder.Property(x => x.TipoConcepto).IsRequired().HasMaxLength(20);
        builder.Property(x => x.UnidadMedida).HasMaxLength(20);
        builder.Property(x => x.Importe).HasColumnType("decimal(10,2)").HasDefaultValue(0m);
        builder.Property(x => x.UsuarioCreacion).HasMaxLength(50);
        builder.Property(x => x.Vigente).IsRequired().HasDefaultValue(true);
        builder.Property(x => x.FechaCreacion).IsRequired().HasDefaultValueSql("NOW()");
        builder.Property(x => x.FechaRegistro).IsRequired().HasDefaultValueSql("NOW()");
    }
}
