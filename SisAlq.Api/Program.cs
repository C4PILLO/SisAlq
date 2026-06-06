using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SisAlq.Api.Data;
using SisAlq.Api.Endpoints;
using SisAlq.Api.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ─── Base de datos ────────────────────────────────────────────
builder.Services.AddDbContext<SisAlqDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ─── CORS ─────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("SisAlqPolicy", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "https://gestion-pro-yw4g.vercel.app"
              )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("SisAlq API — MAEL S.R.L.")
               .WithTheme(ScalarTheme.DeepSpace)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors("SisAlqPolicy");
app.UseAuthentication();
app.UseAuthorization();

// ─── Endpoints ────────────────────────────────────────────────
app.MapGet("/health", () => Results.Ok("healthy")).AllowAnonymous();
app.MapAuthEndpoints();
app.MapInmuebleEndpoints();
app.MapInquilinoEndpoints();

app.Run();