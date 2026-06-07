using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SisAlq.Api.Shared.Data;

namespace SisAlq.Api.Features.Auth;

public static class Login
{
    public record Request(string Usuario, string Contrasena);

    public record Response(
        string Token,
        string NombreUsuario,
        string Rol,
        DateTime Expira
    );

    public static async Task<IResult> Handle(
        Request request,
        SisAlqDbContext db,
        IConfiguration config,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Usuario) ||
            string.IsNullOrWhiteSpace(request.Contrasena))
            return Results.BadRequest(new
            {
                status = 400,
                error = "Bad Request",
                message = "Usuario y contraseña son requeridos."
            });

        var usuario = await db.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.NombreUsuario == request.Usuario && u.Estado, ct);

        if (usuario is null || !BCrypt.Net.BCrypt.Verify(request.Contrasena, usuario.PasswordHash))
            return Results.Unauthorized();

        var (token, expira) = GenerarToken(usuario.NombreUsuario, usuario.Rol.Descripcion, config);

        return Results.Ok(new Response(token, usuario.NombreApellidos, usuario.Rol.Descripcion, expira));
    }

    private static (string Token, DateTime Expira) GenerarToken(
        string nombreUsuario,
        string rol,
        IConfiguration config)
    {
        var key = config["Jwt:Key"]!;
        var issuer = config["Jwt:Issuer"]!;
        var audience = config["Jwt:Audience"]!;
        var minutes = int.Parse(config["Jwt:ExpiresInMinutes"]!);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expira = DateTime.UtcNow.AddMinutes(minutes);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name,             nombreUsuario),
            new Claim(ClaimTypes.Role,             rol),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expira,
            signingCredentials: credentials
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expira);
    }
}