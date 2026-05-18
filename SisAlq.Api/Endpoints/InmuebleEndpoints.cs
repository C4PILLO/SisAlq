using SisAlq.Api.DTOs.Inmueble;
using SisAlq.Api.Services;

namespace SisAlq.Api.Endpoints;

public static class InmuebleEndpoints
{
    public static void MapInmuebleEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/inmuebles")
                       .WithTags("Inmuebles")
                       .RequireAuthorization(); // todos los endpoints requieren JWT

        // GET /api/inmuebles — lista completa con estado e inquilino (HU-004)
        group.MapGet("/", async (IInmuebleService service) =>
        {
            var inmuebles = await service.GetAllAsync();
            return Results.Ok(inmuebles);
        })
        .WithSummary("Listar todos los inmuebles con estado de ocupación")
        .WithName("GetInmuebles");

        // GET /api/inmuebles/{id}
        group.MapGet("/{id:int}", async (int id, IInmuebleService service) =>
        {
            var inmueble = await service.GetByIdAsync(id);
            return inmueble is null
                ? Results.NotFound($"Inmueble con ID {id} no encontrado.")
                : Results.Ok(inmueble);
        })
        .WithSummary("Obtener inmueble por ID")
        .WithName("GetInmuebleById");

        // POST /api/inmuebles — HU-001
        group.MapPost("/", async (CreateInmuebleRequest request, IInmuebleService service) =>
        {
            // Validación manual del modelo
            var validationErrors = ValidateCreate(request);
            if (validationErrors.Count > 0)
                return Results.ValidationProblem(validationErrors);

            // Código único de negocio
            if (await service.CodigoExisteAsync(request.CodigoInmueble))
                return Results.Conflict($"El código '{request.CodigoInmueble.ToUpper()}' ya está registrado.");

            var inmueble = await service.CreateAsync(request);
            return Results.Created($"/api/inmuebles/{inmueble.IdInmueble}", inmueble);
        })
        .WithSummary("Registrar nuevo inmueble (HU-001)")
        .WithName("CreateInmueble")
        .RequireAuthorization(policy => policy.RequireRole("Administrador", "Asistente"));

        // PUT /api/inmuebles/{id}
        group.MapPut("/{id:int}", async (int id, UpdateInmuebleRequest request, IInmuebleService service) =>
        {
            var validationErrors = ValidateUpdate(request);
            if (validationErrors.Count > 0)
                return Results.ValidationProblem(validationErrors);

            var inmueble = await service.UpdateAsync(id, request);
            return inmueble is null
                ? Results.NotFound($"Inmueble con ID {id} no encontrado.")
                : Results.Ok(inmueble);
        })
        .WithSummary("Actualizar inmueble")
        .WithName("UpdateInmueble")
        .RequireAuthorization(policy => policy.RequireRole("Administrador", "Asistente"));

        // DELETE /api/inmuebles/{id}
        group.MapDelete("/{id:int}", async (int id, IInmuebleService service) =>
        {
            // TODO Sprint 2: bloquear delete si tiene contrato activo
            var eliminado = await service.DeleteAsync(id);
            return eliminado
                ? Results.NoContent()
                : Results.NotFound($"Inmueble con ID {id} no encontrado.");
        })
        .WithSummary("Eliminar inmueble")
        .WithName("DeleteInmueble")
        .RequireAuthorization(policy => policy.RequireRole("Administrador"));
    }

    // ─── Validaciones manuales ─────────────────────────────────────────────

    private static Dictionary<string, string[]> ValidateCreate(CreateInmuebleRequest r)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(r.CodigoInmueble))
            errors["CodigoInmueble"] = ["El código del inmueble es requerido."];

        if (string.IsNullOrWhiteSpace(r.DescripcionInmueble))
            errors["DescripcionInmueble"] = ["La descripción es requerida."];

        if (r.PrecioAlquiler <= 0)
            errors["PrecioAlquiler"] = ["El precio de alquiler debe ser mayor a 0."];

        if (r.IdTipoInmueble <= 0)
            errors["IdTipoInmueble"] = ["Debe seleccionar un tipo de inmueble."];

        if (r.IdSector <= 0)
            errors["IdSector"] = ["Debe seleccionar un sector."];

        if (r.IdEstadoInmueble <= 0)
            errors["IdEstadoInmueble"] = ["Debe seleccionar un estado."];

        if (r.IdMoneda <= 0)
            errors["IdMoneda"] = ["Debe seleccionar una moneda."];

        if (r.IncluyeServicios != "S" && r.IncluyeServicios != "N")
            errors["IncluyeServicios"] = ["Debe ser 'S' o 'N'."];

        return errors;
    }

    private static Dictionary<string, string[]> ValidateUpdate(UpdateInmuebleRequest r)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(r.DescripcionInmueble))
            errors["DescripcionInmueble"] = ["La descripción es requerida."];

        if (r.PrecioAlquiler <= 0)
            errors["PrecioAlquiler"] = ["El precio de alquiler debe ser mayor a 0."];

        if (r.IdTipoInmueble <= 0)
            errors["IdTipoInmueble"] = ["Debe seleccionar un tipo de inmueble."];

        if (r.IdSector <= 0)
            errors["IdSector"] = ["Debe seleccionar un sector."];

        if (r.IdEstadoInmueble <= 0)
            errors["IdEstadoInmueble"] = ["Debe seleccionar un estado."];

        if (r.IdMoneda <= 0)
            errors["IdMoneda"] = ["Debe seleccionar una moneda."];

        if (r.IncluyeServicios != "S" && r.IncluyeServicios != "N")
            errors["IncluyeServicios"] = ["Debe ser 'S' o 'N'."];

        return errors;
    }
}