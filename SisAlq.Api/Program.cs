using SisAlq.Api.Features.Auth;
using SisAlq.Api.Features.Cobranzas;
using SisAlq.Api.Features.Configuracion;
using SisAlq.Api.Features.ConceptosConsumo;
using SisAlq.Api.Features.Contratos;
using SisAlq.Api.Features.Inmuebles;
using SisAlq.Api.Features.Inquilinos;
using SisAlq.Api.Features.RecibosConsumo;
using SisAlq.Api.Shared.Extensions;
using SisAlq.Api.Shared.Infrastructure;
using SisAlq.Api.Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ─── Servicios ────────────────────────────────────────────────
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddOpenApiDocs();
builder.Services.AddHttpClient("ApisPeruClient");

var app = builder.Build();

// ─── Seeder ───────────────────────────────────────────────────
try
{
    await app.SeedAdminUserAsync();
    await app.SeedEstadosContratoAsync();
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Error al ejecutar el seeding inicial. La aplicación continuará sin seed.");
}

// ─── Pipeline ─────────────────────────────────────────────────
app.UseErrorHandling();
app.UseHttpsRedirection();
app.UseCors("SisAlqPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseOpenApiDocs();

// ─── Endpoints ────────────────────────────────────────────────
app.MapGet("/health", () => Results.Ok("healthy")).AllowAnonymous();
app.MapAuthEndpoints();
app.MapInmuebleEndpoints();
app.MapInquilinoEndpoints();
app.MapContratoEndpoints();
app.MapConceptoConsumoEndpoints();
app.MapReciboConsumoEndpoints();
app.MapCobranzaEndpoints();
app.MapConfiguracionEndpoints();

app.Run();

