using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAlq.Api.Shared.Models.Catalogos;

namespace SisAlq.Api.Shared.Data.Configurations;

public class TipoInmuebleConfiguration : IEntityTypeConfiguration<TipoInmueble>
{
    public void Configure(EntityTypeBuilder<TipoInmueble> builder)
    {
        builder.ToTable("TIPO_INMUEBLE");
        builder.HasKey(x => x.IdTipoInmueble);
        builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(20);
    }
}

public class EstadoInmuebleConfiguration : IEntityTypeConfiguration<EstadoInmueble>
{
    public void Configure(EntityTypeBuilder<EstadoInmueble> builder)
    {
        builder.ToTable("ESTADO_INMUEBLE");
        builder.HasKey(x => x.IdEstadoInmueble);
        builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(20);
    }
}

public class SectorConfiguration : IEntityTypeConfiguration<Sector>
{
    public void Configure(EntityTypeBuilder<Sector> builder)
    {
        builder.ToTable("SECTOR_ZONA");
        builder.HasKey(x => x.IdSector);
        builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(20);
    }
}

public class TipoClienteConfiguration : IEntityTypeConfiguration<TipoCliente>
{
    public void Configure(EntityTypeBuilder<TipoCliente> builder)
    {
        builder.ToTable("TIPO_CLIENTE");
        builder.HasKey(x => x.IdTipoCliente);
        builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(20);
    }
}

public class TipoDocumentoConfiguration : IEntityTypeConfiguration<TipoDocumento>
{
    public void Configure(EntityTypeBuilder<TipoDocumento> builder)
    {
        builder.ToTable("TIPO_DOCUMENTO");
        builder.HasKey(x => x.IdTDocumento);
        builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(20);
    }
}

public class MonedaConfiguration : IEntityTypeConfiguration<Moneda>
{
    public void Configure(EntityTypeBuilder<Moneda> builder)
    {
        builder.ToTable("MONEDA");
        builder.HasKey(x => x.IdMoneda);
        builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(10);
    }
}

public class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.ToTable("ROLES");
        builder.HasKey(x => x.IdRol);
        builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(150);
        builder.Property(x => x.FechaRegistro).IsRequired().HasDefaultValueSql("NOW()");
    }
}
public class TipoComprobanteConfiguration : IEntityTypeConfiguration<TipoComprobante>
{
    public void Configure(EntityTypeBuilder<TipoComprobante> builder)
    {
        builder.ToTable("TIPO_COMPROBANTE");
        builder.HasKey(x => x.CodigoTD);
        builder.Property(x => x.CodigoTD).HasMaxLength(2).IsRequired();
        builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(30);
        builder.Property(x => x.Correlativo).IsRequired().HasDefaultValue(0);
    }
}

public class BancoConfiguration : IEntityTypeConfiguration<Banco>
{
    public void Configure(EntityTypeBuilder<Banco> builder)
    {
        builder.ToTable("BANCOS");
        builder.HasKey(x => x.CodigoBanco);
        builder.Property(x => x.CodigoBanco).HasMaxLength(2).IsRequired();
        builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Estado).IsRequired().HasMaxLength(1).HasDefaultValue("A");
        builder.Property(x => x.Alias).HasMaxLength(15);
    }
}

public class MedioPagoConfiguration : IEntityTypeConfiguration<MedioPago>
{
    public void Configure(EntityTypeBuilder<MedioPago> builder)
    {
        builder.ToTable("MEDIO_PAGO");
        builder.HasKey(x => x.CodigoMedioPago);
        builder.Property(x => x.CodigoMedioPago).HasMaxLength(2).IsRequired();
        builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(50);
    }
}
