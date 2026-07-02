using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;
using SisAlq.Api.Shared.Models;

namespace SisAlq.Api.Features.Contratos;

public static class CreateContrato
{
    public record Request(
        DateOnly FechaContrato,
        int IdInquilino,
        string? Representante,
        string? TipoNegocio,
        DateOnly FechaInicio,
        int NroMeses,
        int IdMoneda,
        decimal Garantia,
        // Detalle
        int IdInmueble,
        decimal RentaMensual,
        int MesesGarantia,
        string ModalidadPago,
        int CuotasPendientes
    );

    public record Response(
        int IdContrato,
        string NroContrato,
        string NombreInquilino,
        string CodigoInmueble,
        DateOnly FechaInicio,
        DateOnly FechaVcmto,
        int NroMeses,
        decimal RentaMensual,
        decimal Garantia,
        int MesesGarantia,
        string ModalidadPago,
        int CuotasPendientes,
        string EstadoContrato
    );

    public static async Task<IResult> Handle(
        Request req,
        SisAlqDbContext db,
        HttpContext ctx)
    {
        // ── Validar inquilino vigente ────────────────────────────
        var inquilino = await db.Inquilinos
            .FirstOrDefaultAsync(i => i.IdInquilino == req.IdInquilino && i.Vigente);

        if (inquilino is null)
            return Results.BadRequest(new { error = "Inquilino no encontrado o inactivo." });

        // ── Validar inmueble disponible ──────────────────────────
        var inmueble = await db.Inmuebles
            .Include(i => i.EstadoInmueble)
            .FirstOrDefaultAsync(i => i.IdInmueble == req.IdInmueble);

        if (inmueble is null)
            return Results.BadRequest(new { error = "Inmueble no encontrado." });

        if (inmueble.EstadoInmueble.Descripcion != "Disponible")
            return Results.BadRequest(new { error = $"El inmueble no está disponible. Estado actual: {inmueble.EstadoInmueble.Descripcion}." });

        // ── Validar que no tenga contrato vigente ────────────────
        var contratoActivo = await db.ContratosDetalle
            .Include(d => d.Contrato)
            .AnyAsync(d => d.IdInmueble == req.IdInmueble
                        && d.Contrato.IdEstadoContrato == 1); // 1 = Vigente

        if (contratoActivo)
            return Results.Conflict(new { error = "El inmueble ya tiene un contrato vigente." });

        // ── Generar NroContrato ──────────────────────────────────
        var prefijo = inmueble.CodigoInmueble.Length >= 4
            ? inmueble.CodigoInmueble[..4]
            : inmueble.CodigoInmueble.PadRight(4, '0');

        var totalConPrefijo = await db.ContratosCab
            .CountAsync(c => c.NroContrato.StartsWith(prefijo));

        var nroContrato = $"{prefijo}{(totalConPrefijo + 1):D4}";

        // ── Calcular FechaVcmto ──────────────────────────────────
        var fechaVcmto = req.FechaInicio.AddMonths(req.NroMeses);

        // ── Obtener usuario del JWT ──────────────────────────────
        var nombreUsuario = ctx.User.Identity?.Name ?? "admin";
        var usuario = await db.Usuarios
            .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

        if (usuario is null)
            return Results.Unauthorized();

        // ── Crear ContratoCab ────────────────────────────────────
        var contrato = new ContratoCab
        {
            NroContrato      = nroContrato,
            FechaContrato    = req.FechaContrato,
            IdInquilino      = req.IdInquilino,
            Representante    = req.Representante,
            TipoNegocio      = req.TipoNegocio,
            FechaInicio      = req.FechaInicio,
            FechaVcmto       = fechaVcmto,
            IdEstadoContrato = 4, // Doc Pendiente
            NroMeses         = req.NroMeses,
            IdUsuario        = usuario.IdUsuario,
            IdMoneda         = req.IdMoneda,
            Garantia         = req.Garantia,
            MesesGarantia = req.MesesGarantia,
            ModalidadPago = req.ModalidadPago,
            CuotasPendientes = req.CuotasPendientes
        };

        db.ContratosCab.Add(contrato);
        await db.SaveChangesAsync(); // necesitamos IdContrato generado

        // ── Crear ContratoDet ────────────────────────────────────
        var detalle = new ContratoDet
        {
            IdContrato   = contrato.IdContrato,
            IdInmueble   = req.IdInmueble,
            RentaMensual = req.RentaMensual,
            NroMeses     = req.NroMeses,
            NroMesPPago  = req.NroMeses // al crear, igual a NroMeses
        };

        db.ContratosDetalle.Add(detalle);

        await db.SaveChangesAsync();

        // ── Response ─────────────────────────────────────────────
        return Results.Created($"/api/contratos/{contrato.IdContrato}", new Response(
            IdContrato:      contrato.IdContrato,
            NroContrato:     nroContrato,
            NombreInquilino: inquilino.RsocialNApellidos,
            CodigoInmueble:  inmueble.CodigoInmueble,
            FechaInicio:     req.FechaInicio,
            FechaVcmto:      fechaVcmto,
            NroMeses:        req.NroMeses,
            RentaMensual:    req.RentaMensual,
            Garantia:        req.Garantia,
            MesesGarantia: req.MesesGarantia,
            ModalidadPago: req.ModalidadPago,
            CuotasPendientes: req.CuotasPendientes,
            EstadoContrato: "Doc Pendiente"
        ));
    }
}
