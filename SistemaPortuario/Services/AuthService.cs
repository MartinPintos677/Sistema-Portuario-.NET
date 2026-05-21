using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SistemaPortuario.Data;
using SistemaPortuario.DTOs;
using SistemaPortuario.Models;
using SistemaPortuario.Options;
using SistemaPortuario.Security;

namespace SistemaPortuario.Services;

/// <summary>
/// Servicio de autenticación.
/// Valida credenciales, emite JWT, administra refresh tokens y permite crear
/// el primer administrador cuando la base esta vacia.
/// </summary>
public class AuthService(SistemaPortuarioDbContext context, IOptions<JwtOptions> jwtOptions) : IAuthService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private const int RefreshTokenDays = 7;

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default)
    {
        var usuario = await UsuariosConRelaciones()
            .FirstOrDefaultAsync(u => u.Correo == dto.Correo && u.Activo, cancellationToken);

        if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
        {
            return null;
        }

        return await CrearLoginResponseAsync(usuario, cancellationToken: cancellationToken);
    }

    // Flujo bootstrap: solo se permite si aun no existe ningun usuario.
    public async Task<LoginResponseDto?> CrearPrimerAdministradorAsync(PrimerAdministradorRequestDto dto, CancellationToken cancellationToken = default)
    {
        if (await context.Usuarios.AnyAsync(cancellationToken))
        {
            return null;
        }

        var rolAdministrador = await context.Roles
            .FirstAsync(r => r.Nombre == AppRoles.Administrador, cancellationToken);

        var empresa = new Empresa
        {
            Nombre = dto.EmpresaNombre,
            RazonSocial = dto.EmpresaRazonSocial,
            Rut = dto.EmpresaRut,
            TipoEmpresa = dto.EmpresaTipo
        };

        var usuario = new Usuario
        {
            Empresa = empresa,
            IdRol = rolAdministrador.IdRol,
            Cedula = dto.Cedula,
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Correo = dto.Correo,
            Telefono = dto.Telefono,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        context.Empresas.Add(empresa);
        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync(cancellationToken);

        usuario.Rol = rolAdministrador;
        usuario.Empresa = empresa;

        return await CrearLoginResponseAsync(usuario, cancellationToken: cancellationToken);
    }

    public async Task<LoginResponseDto?> RefreshAsync(RefreshTokenRequestDto dto, CancellationToken cancellationToken = default)
    {
        // Los refresh tokens se guardan hasheados para no persistir secretos en texto plano.
        var tokenHash = HashToken(dto.RefreshToken);
        var refreshToken = await context.RefreshTokens
            .Include(t => t.Usuario)
                .ThenInclude(u => u.Empresa)
            .Include(t => t.Usuario)
                .ThenInclude(u => u.Rol)
            .FirstOrDefaultAsync(t => t.TokenHash == tokenHash, cancellationToken);

        if (refreshToken is null || !refreshToken.Activo || !refreshToken.Usuario.Activo)
        {
            return null;
        }

        var nuevoRefreshToken = GenerateRefreshToken();
        refreshToken.FechaRevocacion = DateTime.UtcNow;
        refreshToken.ReemplazadoPorTokenHash = HashToken(nuevoRefreshToken);

        return await CrearLoginResponseAsync(
            refreshToken.Usuario,
            nuevoRefreshToken,
            cancellationToken);
    }

    public async Task LogoutAsync(LogoutRequestDto dto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.RefreshToken))
        {
            return;
        }

        var tokenHash = HashToken(dto.RefreshToken);
        var refreshToken = await context.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == tokenHash && t.FechaRevocacion == null, cancellationToken);

        if (refreshToken is null)
        {
            return;
        }

        refreshToken.FechaRevocacion = DateTime.UtcNow;
        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task<LoginResponseDto> CrearLoginResponseAsync(
        Usuario usuario,
        string? refreshToken = null,
        CancellationToken cancellationToken = default)
    {
        // Cada login/refresh genera access token corto y refresh token rotativo.
        var expira = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes);
        var token = CrearToken(usuario, expira);
        refreshToken ??= GenerateRefreshToken();

        context.RefreshTokens.Add(new RefreshToken
        {
            IdUsuario = usuario.IdUsuario,
            TokenHash = HashToken(refreshToken),
            Expira = DateTime.UtcNow.AddDays(RefreshTokenDays)
        });

        await context.SaveChangesAsync(cancellationToken);
        return new LoginResponseDto(token, refreshToken, expira, usuario.ToDto());
    }

    private string CrearToken(Usuario usuario, DateTime expira)
    {
        // Claims minimos que usan autorizacion, filtros por empresa y auditoría.
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}".Trim()),
            new(ClaimTypes.Email, usuario.Correo),
            new(ClaimTypes.Role, usuario.Rol.Nombre),
            new("empresa_id", usuario.IdEmpresa.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expira,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private IQueryable<Usuario> UsuariosConRelaciones() =>
        context.Usuarios
            .Include(u => u.Empresa)
            .Include(u => u.Rol);

    private static string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    private static string HashToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes);
    }
}

