using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SisAlq.Api.Data;
using SisAlq.Api.Endpoints;
using SisAlq.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ─── Base de datos ────────────────────────────────────────────
builder.Services.AddDbContext<SisAlqDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ─── JWT ──────────────────────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

// ─── Servicios ────────────────────────────────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IInmuebleService, InmuebleService>();
builder.Services.AddScoped<IInquilinoService, InquilinoService>();

// ─── OpenAPI ──────────────────────────────────────────────────
builder.Services.AddOpenApi();

var app = builder.Build();

// ─── Seeder: usuario admin inicial ───────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SisAlqDbContext>();
    if (!db.Usuarios.Any())
    {
        db.Usuarios.Add(new SisAlq.Api.Models.Usuario
        {
            NombreUsuario = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            NombreApellidos = "Administrador SisAlq",
            IdRol = 1,
            Estado = true,
            FechaRegistro = DateTime.UtcNow
        });
        db.SaveChanges();
    }
}

// ─── Pipeline HTTP ────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// ─── Endpoints ────────────────────────────────────────────────
app.MapAuthEndpoints();
app.MapInmuebleEndpoints(); 
app.MapInquilinoEndpoints();

// ─── Health check ─────────────────────────────────────────────
app.MapGet("/health", () => Results.Ok("healthy")).AllowAnonymous();

app.Run();