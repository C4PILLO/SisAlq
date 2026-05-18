using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SisAlq.Api.Data;
using SisAlq.Api.DTOs.Auth;

namespace SisAlq.Api.Services;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}

public class AuthService : IAuthService
{
    private readonly SisAlqDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(SisAlqDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var usuario = await _db.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.NombreUsuario == request.Usuario && u.Estado);

        if (usuario is null)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(request.Contrasena, usuario.PasswordHash))
            return null;

        var token = GenerarToken(usuario.NombreUsuario, usuario.Rol.Descripcion);

        return new LoginResponse
        {
            Token = token.Token,
            NombreUsuario = usuario.NombreApellidos,
            Rol = usuario.Rol.Descripcion,
            Expira = token.Expira
        };
    }

    private (string Token, DateTime Expira) GenerarToken(string nombreUsuario, string rol)
    {
        var key = _config["Jwt:Key"]!;
        var issuer = _config["Jwt:Issuer"]!;
        var audience = _config["Jwt:Audience"]!;
        var minutes = int.Parse(_config["Jwt:ExpiresInMinutes"]!);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expira = DateTime.UtcNow.AddMinutes(minutes);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name,                   nombreUsuario),
            new Claim(ClaimTypes.Role,                   rol),
            new Claim(JwtRegisteredClaimNames.Jti,       Guid.NewGuid().ToString())
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