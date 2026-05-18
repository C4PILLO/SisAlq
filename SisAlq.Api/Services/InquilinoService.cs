using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Data;
using SisAlq.Api.DTOs.Inquilino;
using SisAlq.Api.Models;

namespace SisAlq.Api.Services;

public interface IInquilinoService
{
    Task<IEnumerable<InquilinoResponse>> GetAllAsync();
    Task<InquilinoResponse?> GetByIdAsync(int id);
    Task<InquilinoResponse> CreateAsync(CreateInquilinoRequest request);
    Task<InquilinoResponse?> UpdateAsync(int id, UpdateInquilinoRequest request);
    Task<bool> DocumentoExisteAsync(string nroDocumento, int? excluirId = null);
    Task<string?> ValidarTipoClienteDocumentoAsync(int idTipoCliente, int idTDocumento);
}

public class InquilinoService : IInquilinoService
{
    private readonly SisAlqDbContext _db;

    public InquilinoService(SisAlqDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<InquilinoResponse>> GetAllAsync()
    {
        return await _db.Inquilinos
            .Include(i => i.TipoCliente)
            .Include(i => i.TipoDocumento)
            .OrderBy(i => i.RsocialNApellidos)
            .Select(i => MapToResponse(i))
            .ToListAsync();
    }

    public async Task<InquilinoResponse?> GetByIdAsync(int id)
    {
        var inquilino = await _db.Inquilinos
            .Include(i => i.TipoCliente)
            .Include(i => i.TipoDocumento)
            .FirstOrDefaultAsync(i => i.IdInquilino == id);

        return inquilino is null ? null : MapToResponse(inquilino);
    }

    public async Task<InquilinoResponse> CreateAsync(CreateInquilinoRequest request)
    {
        var inquilino = new Models.Inquilino
        {
            IdTipoCliente = request.IdTipoCliente,
            IdTDocumento = request.IdTDocumento,
            NroDocumento = request.NroDocumento.Trim(),
            RsocialNApellidos = request.RsocialNApellidos.Trim(),
            CelularTelefono = request.CelularTelefono.Trim(),
            Direccion = request.Direccion.Trim(),
            Correo = request.Correo.Trim().ToLower(),
            Referencia = request.Referencia?.Trim(),
            Vigente = true,
            FechaRegistro = DateTime.UtcNow
        };

        _db.Inquilinos.Add(inquilino);
        await _db.SaveChangesAsync();

        // Recargar propiedades de navegación
        await _db.Entry(inquilino).Reference(i => i.TipoCliente).LoadAsync();
        await _db.Entry(inquilino).Reference(i => i.TipoDocumento).LoadAsync();

        return MapToResponse(inquilino);
    }

    public async Task<InquilinoResponse?> UpdateAsync(int id, UpdateInquilinoRequest request)
    {
        var inquilino = await _db.Inquilinos
            .Include(i => i.TipoCliente)
            .Include(i => i.TipoDocumento)
            .FirstOrDefaultAsync(i => i.IdInquilino == id);

        if (inquilino is null) return null;

        inquilino.RsocialNApellidos = request.RsocialNApellidos.Trim();
        inquilino.CelularTelefono = request.CelularTelefono.Trim();
        inquilino.Direccion = request.Direccion.Trim();
        inquilino.Correo = request.Correo.Trim().ToLower();
        inquilino.Referencia = request.Referencia?.Trim();
        inquilino.Vigente = request.Vigente;

        await _db.SaveChangesAsync();

        // Recargar por si acaso (navegación puede quedar stale)
        await _db.Entry(inquilino).Reference(i => i.TipoCliente).LoadAsync();
        await _db.Entry(inquilino).Reference(i => i.TipoDocumento).LoadAsync();

        return MapToResponse(inquilino);
    }

    public async Task<bool> DocumentoExisteAsync(string nroDocumento, int? excluirId = null)
    {
        var nroNormalizado = nroDocumento.Trim();
        return await _db.Inquilinos.AnyAsync(i =>
            i.NroDocumento == nroNormalizado &&
            (excluirId == null || i.IdInquilino != excluirId));
    }

    /// <summary>
    /// Valida que el tipo de documento sea coherente con el tipo de cliente.
    /// Retorna mensaje de error si hay inconsistencia, null si es válido.
    /// Regla: DNI(1) → solo Natural(1) | RUC(2) → solo Juridica(2) | CE(3) → ambos
    /// </summary>
    public Task<string?> ValidarTipoClienteDocumentoAsync(int idTipoCliente, int idTDocumento)
    {
        string? error = (idTipoCliente, idTDocumento) switch
        {
            (1, 2) => "Una persona Natural no puede tener RUC como documento de identidad.",
            (2, 1) => "Una persona Jurídica no puede tener DNI como documento de identidad.",
            _ => null
        };
        return Task.FromResult(error);
    }

    private static InquilinoResponse MapToResponse(Models.Inquilino i) => new()
    {
        IdInquilino = i.IdInquilino,
        NroDocumento = i.NroDocumento,
        RsocialNApellidos = i.RsocialNApellidos,
        CelularTelefono = i.CelularTelefono,
        Direccion = i.Direccion,
        Correo = i.Correo,
        Referencia = i.Referencia,
        Vigente = i.Vigente,
        FechaRegistro = i.FechaRegistro,
        TipoCliente = i.TipoCliente.Descripcion,
        TipoDocumento = i.TipoDocumento.Descripcion
    };
}