using SisAlq.Api.DTOs.Inquilino;
using SisAlq.Api.Services;

namespace SisAlq.Api.Endpoints;

public static class InquilinoEndpoints
{
    public static void MapInquilinoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/inquilinos")
                       .WithTags("Inquilinos")
                       .RequireAuthorization();

        // GET /api/inquilinos
        group.MapGet("/", async (IInquilinoService service) =>
        {
            var inquilinos = await service.GetAllAsync();
            return Results.Ok(inquilinos);
        })
        .WithSummary("Listar todos los inquilinos")
        .WithName("GetInquilinos");

        // GET /api/inquilinos/{id}
        group.MapGet("/{id:int}", async (int id, IInquilinoService service) =>
        {
            var inquilino = await service.GetByIdAsync(id);
            return inquilino is null
                ? Results.NotFound($"Inquilino con ID {id} no encontrado.")
                : Results.Ok(inquilino);
        })
        .WithSummary("Obtener inquilino por ID")
        .WithName("GetInquilinoById");

        // POST /api/inquilinos — HU-002
        group.MapPost("/", async (CreateInquilinoRequest request, IInquilinoService service) =>
        {
            var validationErrors = ValidateCreate(request);
            if (validationErrors.Count > 0)
                return Results.ValidationProblem(validationErrors);

            // Validar coherencia tipo cliente / tipo documento
            var errorDocumento = await service.ValidarTipoClienteDocumentoAsync(
                request.IdTipoCliente, request.IdTDocumento);
            if (errorDocumento is not null)
                return Results.BadRequest(errorDocumento);

            // Documento único
            if (await service.DocumentoExisteAsync(request.NroDocumento))
                return Results.Conflict($"El documento '{request.NroDocumento}' ya está registrado.");

            var inquilino = await service.CreateAsync(request);
            return Results.Created($"/api/inquilinos/{inquilino.IdInquilino}", inquilino);
        })
        .WithSummary("Registrar nuevo inquilino (HU-002)")
        .WithName("CreateInquilino")
        .RequireAuthorization(policy => policy.RequireRole("Administrador", "Asistente"));

        // PUT /api/inquilinos/{id}
        group.MapPut("/{id:int}", async (int id, UpdateInquilinoRequest request, IInquilinoService service) =>
        {
            var validationErrors = ValidateUpdate(request);
            if (validationErrors.Count > 0)
                return Results.ValidationProblem(validationErrors);

            var inquilino = await service.UpdateAsync(id, request);
            return inquilino is null
                ? Results.NotFound($"Inquilino con ID {id} no encontrado.")
                : Results.Ok(inquilino);
        })
        .WithSummary("Actualizar datos del inquilino")
        .WithName("UpdateInquilino")
        .RequireAuthorization(policy => policy.RequireRole("Administrador", "Asistente"));

        // ─── Validaciones manuales ─────────────────────────────────────────────

        static Dictionary<string, string[]> ValidateCreate(CreateInquilinoRequest r)
        {
            var errors = new Dictionary<string, string[]>();

            if (r.IdTipoCliente <= 0)
                errors["IdTipoCliente"] = ["Debe seleccionar un tipo de cliente."];

            if (r.IdTDocumento <= 0)
                errors["IdTDocumento"] = ["Debe seleccionar un tipo de documento."];

            if (string.IsNullOrWhiteSpace(r.NroDocumento))
                errors["NroDocumento"] = ["El número de documento es requerido."];

            if (string.IsNullOrWhiteSpace(r.RsocialNApellidos))
                errors["RsocialNApellidos"] = ["El nombre o razón social es requerido."];

            if (string.IsNullOrWhiteSpace(r.CelularTelefono))
                errors["CelularTelefono"] = ["El celular o teléfono es requerido."];

            if (string.IsNullOrWhiteSpace(r.Direccion))
                errors["Direccion"] = ["La dirección es requerida."];

            if (string.IsNullOrWhiteSpace(r.Correo))
                errors["Correo"] = ["El correo electrónico es requerido."];
            else if (!r.Correo.Contains('@'))
                errors["Correo"] = ["El correo electrónico no tiene un formato válido."];

            return errors;
        }

        static Dictionary<string, string[]> ValidateUpdate(UpdateInquilinoRequest r)
        {
            var errors = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(r.RsocialNApellidos))
                errors["RsocialNApellidos"] = ["El nombre o razón social es requerido."];

            if (string.IsNullOrWhiteSpace(r.CelularTelefono))
                errors["CelularTelefono"] = ["El celular o teléfono es requerido."];

            if (string.IsNullOrWhiteSpace(r.Direccion))
                errors["Direccion"] = ["La dirección es requerida."];

            if (string.IsNullOrWhiteSpace(r.Correo))
                errors["Correo"] = ["El correo electrónico es requerido."];
            else if (!r.Correo.Contains('@'))
                errors["Correo"] = ["El correo electrónico no tiene un formato válido."];

            return errors;
        }
    }
}