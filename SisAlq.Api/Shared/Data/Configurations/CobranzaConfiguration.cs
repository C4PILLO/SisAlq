using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAlq.Api.Shared.Models;

namespace SisAlq.Api.Shared.Data.Configurations;

public class CobranzaCabConfiguration : IEntityTypeConfiguration<CobranzaCab>
{
    public void Configure(EntityTypeBuilder<CobranzaCab> builder)
    {
        builder.ToTable("COBRANZA_CAB");
        builder.HasKey(x => x.IdCobranza);
        builder.Property(x => x.FechaCobro).IsRequired().HasDefaultValueSql("NOW()");
        builder.Property(x => x.TotalCobrado).HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(x => x.Mora).HasColumnType("decimal(12,2)").HasDefaultValue(0m);
        builder.Property(x => x.Observacion).HasMaxLength(300);
        builder.Property(x => x.Usuario).IsRequired().HasMaxLength(15);
        builder.Property(x => x.Estado).IsRequired().HasMaxLength(1).HasDefaultValue("A");

        builder.HasOne(x => x.Inquilino).WithMany().HasForeignKey(x => x.IdInquilino);
        builder.HasOne(x => x.Moneda).WithMany().HasForeignKey(x => x.IdMoneda);
    }
}

public class CobranzaDetConfiguration : IEntityTypeConfiguration<CobranzaDet>
{
    public void Configure(EntityTypeBuilder<CobranzaDet> builder)
    {
        builder.ToTable("COBRANZA_DET");
        builder.HasKey(x => new { x.IdCobranza, x.Secuencia });

        builder.Property(x => x.CodigoTD).IsRequired().HasMaxLength(2);
        builder.Property(x => x.NumeroDoc).IsRequired().HasMaxLength(12);
        builder.Property(x => x.CodigoMPago).IsRequired().HasMaxLength(2);
        builder.Property(x => x.CodigoBanco).HasMaxLength(2);
        builder.Property(x => x.AliasBanco).HasMaxLength(30);
        builder.Property(x => x.NroOperacion).HasMaxLength(20);
        builder.Property(x => x.Importe).HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(x => x.Mora).HasColumnType("decimal(12,2)").HasDefaultValue(0m);
        builder.Property(x => x.Descuento).HasColumnType("decimal(12,2)").HasDefaultValue(0m);
        builder.Property(x => x.TotalPagado).HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(x => x.EstadoPago).IsRequired().HasMaxLength(20);

        builder.HasOne(x => x.Cabecera).WithMany(c => c.Detalle).HasForeignKey(x => x.IdCobranza);
        builder.HasOne(x => x.TipoComprobante).WithMany().HasForeignKey(x => x.CodigoTD);
        builder.HasOne(x => x.MedioPago).WithMany().HasForeignKey(x => x.CodigoMPago);
        builder.HasOne(x => x.Banco).WithMany().HasForeignKey(x => x.CodigoBanco).IsRequired(false);
    }
}
