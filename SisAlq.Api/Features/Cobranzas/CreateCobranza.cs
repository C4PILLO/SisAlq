using Microsoft.EntityFrameworkCore;
using SisAlq.Api.Shared.Data;
using SisAlq.Api.Shared.Models;

namespace SisAlq.Api.Features.Cobranzas;

public record CobranzaItemRequest(string CodigoTD, int NroDocumento, decimal ImporteAPagar);

public record CreateCobranzaRequest(
    int IdInquilino,
    string CodigoMPago,
    string? CodigoBanco,
    string? NroOperacion,
    string? Observacion,
    string Usuario,
    List<CobranzaItemRequest> Documentos
);

public static class CreateCobranza
{
    public static async Task<IResult> Handle(CreateCobranzaRequest request, SisAlqDbContext db)
    {
        if (request.Documentos is null || request.Documentos.Count == 0)
            return Results.BadRequest(new { mensaje = "Debe incluir al menos un documento a cobrar." });

        var medioPago = await db.MediosPago.FindAsync(request.CodigoMPago);
        if (medioPago is null)
            return Results.BadRequest(new { mensaje = "Medio de pago no valido." });

        switch (request.CodigoMPago)
        {
            case "EF":
                break;
            case "DP":
            case "TR":
                if (string.IsNullOrWhiteSpace(request.CodigoBanco))
                    return Results.BadRequest(new { mensaje = "El banco es obligatorio para Deposito o Transferencia." });
                if (string.IsNullOrWhiteSpace(request.NroOperacion))
                    return Results.BadRequest(new { mensaje = "El numero de operacion es obligatorio." });
                break;
            case "YA":
            case "PL":
                if (string.IsNullOrWhiteSpace(request.NroOperacion))
                    return Results.BadRequest(new { mensaje = "El numero de operacion es obligatorio para Yape/Plin." });
                break;
            case "CH":
                if (string.IsNullOrWhiteSpace(request.NroOperacion))
                    return Results.BadRequest(new { mensaje = "El numero de cheque es obligatorio." });
                break;
            default:
                return Results.BadRequest(new { mensaje = "Medio de pago no reconocido." });
        }

        string? aliasBanco = null;
        if (!string.IsNullOrWhiteSpace(request.CodigoBanco))
        {
            var banco = await db.Bancos.FindAsync(request.CodigoBanco);
            if (banco is null)
                return Results.BadRequest(new { mensaje = "Banco no valido." });
            aliasBanco = banco.Alias;
        }

        var docsACobrar = new List<(DocumentoXCobrar Doc, decimal ImporteAPagar)>();

        foreach (var item in request.Documentos)
        {
            var doc = await db.DocumentosXCobrar.FirstOrDefaultAsync(d =>
                d.IdInquilino == request.IdInquilino &&
                d.CodigoTD == item.CodigoTD &&
                d.NroDocumento == item.NroDocumento);

            if (doc is null)
                return Results.NotFound(new { mensaje = string.Format("Documento {0}-{1} no encontrado.", item.CodigoTD, item.NroDocumento) });

            if (doc.Saldo <= 0)
                return Results.Conflict(new { mensaje = string.Format("El documento {0}-{1} ya esta cancelado.", item.CodigoTD, item.NroDocumento) });

            if (item.ImporteAPagar <= 0)
                return Results.BadRequest(new { mensaje = "El importe a pagar debe ser mayor a cero." });

            if (item.ImporteAPagar > doc.Saldo)
                return Results.BadRequest(new { mensaje = string.Format("El importe a pagar (S/ {0}) no puede ser mayor al saldo (S/ {1}) del documento {2}-{3}.", item.ImporteAPagar, doc.Saldo, item.CodigoTD, item.NroDocumento) });

            docsACobrar.Add((doc, item.ImporteAPagar));
        }

        var idMoneda = docsACobrar[0].Doc.IdMoneda;
        if (docsACobrar.Any(x => x.Doc.IdMoneda != idMoneda))
            return Results.BadRequest(new { mensaje = "Todos los documentos de una misma cobranza deben estar en la misma moneda." });

        var totalCobrado = docsACobrar.Sum(x => x.ImporteAPagar);

        var strategy = db.Database.CreateExecutionStrategy();
        IResult result = Results.Problem("No se pudo registrar la cobranza.");

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await db.Database.BeginTransactionAsync();

            var cobranza = new CobranzaCab
            {
                FechaCobro   = DateTime.UtcNow,
                IdInquilino  = request.IdInquilino,
                IdMoneda     = idMoneda,
                TotalCobrado = totalCobrado,
                Mora         = 0,
                Observacion  = request.Observacion,
                Usuario      = request.Usuario,
                Estado       = "A"
            };

            db.CobranzasCab.Add(cobranza);
            await db.SaveChangesAsync();

            int secuencia = 1;
            var detalleResumen = new List<object>();

            foreach (var (doc, importeAPagar) in docsACobrar)
            {
                var nuevoSaldo = doc.Saldo - importeAPagar;

                db.CobranzasDet.Add(new CobranzaDet
                {
                    IdCobranza   = cobranza.IdCobranza,
                    Secuencia    = secuencia++,
                    CodigoTD     = doc.CodigoTD,
                    NumeroDoc    = doc.NroDocumento.ToString(),
                    CodigoMPago  = request.CodigoMPago,
                    CodigoBanco  = request.CodigoBanco,
                    AliasBanco   = aliasBanco,
                    NroOperacion = request.NroOperacion,
                    Importe      = doc.Importe,
                    Mora         = 0,
                    Descuento    = 0,
                    TotalPagado  = importeAPagar,
                    EstadoPago   = nuevoSaldo <= 0 ? "Cancelado" : "Parcial"
                });

                doc.Saldo = nuevoSaldo;

                detalleResumen.Add(new
                {
                    doc.CodigoTD,
                    doc.NroDocumento,
                    ImportePagado = importeAPagar,
                    SaldoRestante = nuevoSaldo
                });
            }

            await db.SaveChangesAsync();
            await transaction.CommitAsync();

            result = Results.Created(
                string.Format("/api/cobranzas/{0}", cobranza.IdCobranza),
                new
                {
                    cobranza.IdCobranza,
                    cobranza.FechaCobro,
                    cobranza.TotalCobrado,
                    documentos = detalleResumen
                });
        });

        return result;
    }
}
