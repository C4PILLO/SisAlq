using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Models;
using SisAlq.Api.Models.Catalogos;

namespace SisAlq.Api.Data;

public class SisAlqDbContext : DbContext
{
    public SisAlqDbContext(DbContextOptions<SisAlqDbContext> options)
        : base(options) { }

    // Tablas principales
    public DbSet<Inmueble> Inmuebles => Set<Inmueble>();
    public DbSet<Inquilino> Inquilinos => Set<Inquilino>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    // Tablas de catálogo
    public DbSet<TipoInmueble> TiposInmueble => Set<TipoInmueble>();
    public DbSet<EstadoInmueble> EstadosInmueble => Set<EstadoInmueble>();
    public DbSet<Sector> Sectores => Set<Sector>();
    public DbSet<TipoCliente> TiposCliente => Set<TipoCliente>();
    public DbSet<TipoDocumento> TiposDocumento => Set<TipoDocumento>();
    public DbSet<Moneda> Monedas => Set<Moneda>();
    public DbSet<Rol> Roles => Set<Rol>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar configuraciones de la carpeta Configurations/
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SisAlqDbContext).Assembly);
    }
}