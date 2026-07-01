using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Models;
using SisAlq.Api.Shared.Models.Catalogos;

namespace SisAlq.Api.Shared.Data;

public class SisAlqDbContext : DbContext
{
    public SisAlqDbContext(DbContextOptions<SisAlqDbContext> options)
        : base(options) { }

    // Tablas principales
    public DbSet<Inmueble> Inmuebles => Set<Inmueble>();
    public DbSet<Inquilino> Inquilinos => Set<Inquilino>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<ContratoCab> ContratosCab => Set<ContratoCab>();
    public DbSet<ContratoDet> ContratosDetalle => Set<ContratoDet>();
    public DbSet<ConceptoConsumoServicio> ConceptosConsumo => Set<ConceptoConsumoServicio>();

    // Tablas de catálogo
    public DbSet<TipoInmueble> TiposInmueble => Set<TipoInmueble>();
    public DbSet<EstadoInmueble> EstadosInmueble => Set<EstadoInmueble>();
    public DbSet<Sector> Sectores => Set<Sector>();
    public DbSet<TipoCliente> TiposCliente => Set<TipoCliente>();
    public DbSet<TipoDocumento> TiposDocumento => Set<TipoDocumento>();
    public DbSet<Moneda> Monedas => Set<Moneda>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<EstadoContrato> EstadosContrato => Set<EstadoContrato>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SisAlqDbContext).Assembly);
    }
}