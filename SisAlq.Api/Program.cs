using SisAlq.Api.Features.Auth;
using SisAlq.Api.Features.Configuracion;
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
await app.SeedAdminUserAsync();
await app.SeedEstadosContratoAsync();

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
app.MapReciboConsumoEndpoints();
app.MapConfiguracionEndpoints();

app.Run();
