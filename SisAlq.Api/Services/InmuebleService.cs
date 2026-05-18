using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Data;
using SisAlq.Api.DTOs.Inmueble;
using SisAlq.Api.Models;

namespace SisAlq.Api.Services;

public interface IInmuebleService
{
    Task<IEnumerable<InmuebleResponse>> GetAllAsync();
    Task<InmuebleResponse?> GetByIdAsync(int id);
    Task<InmuebleResponse> CreateAsync(CreateInmuebleRequest request);
    Task<InmuebleResponse?> UpdateAsync(int id, UpdateInmuebleRequest request);
    Task<bool> DeleteAsync(int id);
    Task<bool> CodigoExisteAsync(string codigo, int? excluirId = null);
}

public class InmuebleService : IInmuebleService
{
    private readonly SisAlqDbContext _db;

    public InmuebleService(SisAlqDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<InmuebleResponse>> GetAllAsync()
    {
        return await _db.Inmuebles
            .Include(i => i.TipoInmueble)
            .Include(i => i.Sector)
            .Include(i => i.EstadoInmueble)
            .Include(i => i.Moneda)
            .Select(i => MapToResponse(i))
            .ToListAsync();
    }

    public async Task<InmuebleResponse?> GetByIdAsync(int id)
    {
        var inmueble = await _db.Inmuebles
            .Include(i => i.TipoInmueble)
            .Include(i => i.Sector)
            .Include(i => i.EstadoInmueble)
            .Include(i => i.Moneda)
            .FirstOrDefaultAsync(i => i.IdInmueble == id);

        return inmueble is null ? null : MapToResponse(inmueble);
    }

    public async Task<InmuebleResponse> CreateAsync(CreateInmuebleRequest request)
    {
        var inmueble = new Inmueble
        {
            IdTipoInmueble = request.IdTipoInmueble,
            IdSector = request.IdSector,
            IdEstadoInmueble = request.IdEstadoInmueble,
            IdMoneda = request.IdMoneda,
            CodigoInmueble = request.CodigoInmueble.Trim().ToUpper(),
            DescripcionInmueble = request.DescripcionInmueble.Trim(),
            PisoInmueble = request.PisoInmueble?.Trim(),
            PrecioAlquiler = request.PrecioAlquiler,
            IncluyeServicios = request.IncluyeServicios[0],
            Observaciones = request.Observaciones?.Trim(),
            FechaRegistro = DateTime.UtcNow
        };

        _db.Inmuebles.Add(inmueble);
        await _db.SaveChangesAsync();

        // Recargar con navegaciones para el response
        await _db.Entry(inmueble).Reference(i => i.TipoInmueble).LoadAsync();
        await _db.Entry(inmueble).Reference(i => i.Sector).LoadAsync();
        await _db.Entry(inmueble).Reference(i => i.EstadoInmueble).LoadAsync();
        await _db.Entry(inmueble).Reference(i => i.Moneda).LoadAsync();

        return MapToResponse(inmueble);
    }

    public async Task<InmuebleResponse?> UpdateAsync(int id, UpdateInmuebleRequest request)
    {
        var inmueble = await _db.Inmuebles
            .Include(i => i.TipoInmueble)
            .Include(i => i.Sector)
            .Include(i => i.EstadoInmueble)
            .Include(i => i.Moneda)
            .FirstOrDefaultAsync(i => i.IdInmueble == id);

        if (inmueble is null) return null;

        inmueble.IdTipoInmueble = request.IdTipoInmueble;
        inmueble.IdSector = request.IdSector;
        inmueble.IdEstadoInmueble = request.IdEstadoInmueble;
        inmueble.IdMoneda = request.IdMoneda;
        inmueble.DescripcionInmueble = request.DescripcionInmueble.Trim();
        inmueble.PisoInmueble = request.PisoInmueble?.Trim();
        inmueble.PrecioAlquiler = request.PrecioAlquiler;
        inmueble.IncluyeServicios = request.IncluyeServicios[0];
        inmueble.Observaciones = request.Observaciones?.Trim();

        await _db.SaveChangesAsync();

        // Recargar navegaciones porque los IDs de FK cambiaron
        await _db.Entry(inmueble).Reference(i => i.TipoInmueble).LoadAsync();
        await _db.Entry(inmueble).Reference(i => i.Sector).LoadAsync();
        await _db.Entry(inmueble).Reference(i => i.EstadoInmueble).LoadAsync();
        await _db.Entry(inmueble).Reference(i => i.Moneda).LoadAsync();

        return MapToResponse(inmueble);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var inmueble = await _db.Inmuebles.FindAsync(id);
        if (inmueble is null) return false;

        _db.Inmuebles.Remove(inmueble);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CodigoExisteAsync(string codigo, int? excluirId = null)
    {
        var codigoNormalizado = codigo.Trim().ToUpper();
        return await _db.Inmuebles.AnyAsync(i =>
            i.CodigoInmueble == codigoNormalizado &&
            (excluirId == null || i.IdInmueble != excluirId));
    }

    private static InmuebleResponse MapToResponse(Inmueble i) => new()
    {
        IdInmueble = i.IdInmueble,
        CodigoInmueble = i.CodigoInmueble,
        DescripcionInmueble = i.DescripcionInmueble,
        PisoInmueble = i.PisoInmueble,
        PrecioAlquiler = i.PrecioAlquiler,
        IncluyeServicios = i.IncluyeServicios.ToString(),
        Observaciones = i.Observaciones,
        FechaRegistro = i.FechaRegistro,
        TipoInmueble = i.TipoInmueble.Descripcion,
        Sector = i.Sector.Descripcion,
        EstadoInmueble = i.EstadoInmueble.Descripcion,
        Moneda = i.Moneda.Descripcion,
        InquilinoActual = null  // Se implementa en Sprint 2 con contratos
    };
}