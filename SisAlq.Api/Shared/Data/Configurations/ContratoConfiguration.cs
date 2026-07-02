using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAlq.Api.Shared.Models;
using SisAlq.Api.Shared.Models.Catalogos;

namespace SisAlq.Api.Shared.Data.Configurations;

public class ContratoCabConfiguration : IEntityTypeConfiguration<ContratoCab>
{
    public void Configure(EntityTypeBuilder<ContratoCab> builder)
    {
        builder.ToTable("CONTRATO_CAB");
        builder.HasKey(x => x.IdContrato);

        builder.Property(x => x.NroContrato).IsRequired().HasMaxLength(10);
        builder.HasIndex(x => x.NroContrato).IsUnique();
        builder.Property(x => x.FechaContrato).IsRequired();
        builder.Property(x => x.Representante).HasMaxLength(70);
        builder.Property(x => x.TipoNegocio).HasMaxLength(100);
        builder.Property(x => x.FechaInicio).IsRequired();
        builder.Property(x => x.FechaVcmto).IsRequired();
        builder.Property(x => x.NroMeses).IsRequired();
        builder.Property(x => x.Garantia).IsRequired().HasColumnType("decimal(10,2)");
        builder.Property(x => x.MesesGarantia).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.ModalidadPago).IsRequired().HasMaxLength(20).HasDefaultValue("Adelantado");
        builder.Property(x => x.CuotasPendientes).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.FechaRegistro).IsRequired().HasDefaultValueSql("NOW()");

        builder.HasOne(x => x.Inquilino).WithMany().HasForeignKey(x => x.IdInquilino);
        builder.HasOne(x => x.EstadoContrato).WithMany().HasForeignKey(x => x.IdEstadoContrato);
        builder.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.IdUsuario);
        builder.HasOne(x => x.Moneda).WithMany().HasForeignKey(x => x.IdMoneda);
    }
}

public class ContratoDetConfiguration : IEntityTypeConfiguration<ContratoDet>
{
    public void Configure(EntityTypeBuilder<ContratoDet> builder)
    {
        builder.ToTable("CONTRATO_DET");
        builder.HasKey(x => x.IdContratoDet);

        builder.Property(x => x.RentaMensual).IsRequired().HasColumnType("decimal(10,2)");
        builder.Property(x => x.NroMeses).IsRequired();
        builder.Property(x => x.NroMesPPago).IsRequired();

        builder.HasOne(x => x.Contrato).WithMany(x => x.Detalle).HasForeignKey(x => x.IdContrato);
        builder.HasOne(x => x.Inmueble).WithMany().HasForeignKey(x => x.IdInmueble);
    }
}

public class EstadoContratoConfiguration : IEntityTypeConfiguration<EstadoContrato>
{
    public void Configure(EntityTypeBuilder<EstadoContrato> builder)
    {
        builder.ToTable("ESTADO_CONTRATO");
        builder.HasKey(x => x.IdEstadoContrato);
        builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(20);
    }
}