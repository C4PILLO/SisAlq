using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAlq.Api.Shared.Models;

namespace SisAlq.Api.Shared.Data.Configurations;

public class ConfiguracionParametroConfiguration : IEntityTypeConfiguration<ConfiguracionParametro>
{
    public void Configure(EntityTypeBuilder<ConfiguracionParametro> builder)
    {
        builder.ToTable("CONFIGURACION_PARAMETROS");
        builder.HasKey(x => x.IdParametro);
        builder.Property(x => x.Clave).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.Clave).IsUnique();
        builder.Property(x => x.Valor).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Descripcion).HasMaxLength(200);
        builder.Property(x => x.FechaRegistro).IsRequired().HasDefaultValueSql("NOW()");
    }
}
