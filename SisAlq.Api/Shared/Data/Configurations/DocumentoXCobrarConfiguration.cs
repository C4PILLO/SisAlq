using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAlq.Api.Shared.Models;

namespace SisAlq.Api.Shared.Data.Configurations;

public class DocumentoXCobrarConfiguration : IEntityTypeConfiguration<DocumentoXCobrar>
{
    public void Configure(EntityTypeBuilder<DocumentoXCobrar> builder)
    {
        builder.ToTable("DOCUMENTOS_X_COBRAR");
        builder.HasKey(x => new { x.IdInquilino, x.CodigoTD, x.NroDocumento });

        builder.Property(x => x.CodigoTD).IsRequired().HasMaxLength(2);
        builder.Property(x => x.FechaEmision).IsRequired();
        builder.Property(x => x.FechaVcmto).IsRequired();
        builder.Property(x => x.Importe).HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(x => x.Saldo).HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(x => x.Usuario).IsRequired().HasMaxLength(15);

        builder.HasOne(x => x.Inquilino).WithMany().HasForeignKey(x => x.IdInquilino);
        builder.HasOne(x => x.TipoComprobante).WithMany().HasForeignKey(x => x.CodigoTD);
        builder.HasOne(x => x.Moneda).WithMany().HasForeignKey(x => x.IdMoneda);
    }
}
