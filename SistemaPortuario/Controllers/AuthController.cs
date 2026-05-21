using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPortuario.DTOs;
using SistemaPortuario.Services;

namespace SistemaPortuario.Controllers;

[ApiController]
[Route("api/[controller]")]
/// <summary>
/// Endpoints de autenticacion, refresh token y bootstrap del primer administrador.
/// </summary>
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(dto, cancellationToken);
        return result is null ? Unauthorized("Credenciales invalidas.") : Ok(result);
    }

    [HttpPost("primer-administrador")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> CrearPrimerAdministrador(PrimerAdministradorRequestDto dto, CancellationToken cancellationToken)
    {
        var result = await authService.CrearPrimerAdministradorAsync(dto, cancellationToken);
        return result is null
            ? Conflict("Ya existe al menos un usuario. Use un usuario administrador para crear nuevas cuentas.")
            : Ok(result);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Refresh(RefreshTokenRequestDto dto, CancellationToken cancellationToken)
    {
        var result = await authService.RefreshAsync(dto, cancellationToken);
        return result is null ? Unauthorized("La sesion expiro. Inicie sesion nuevamente.") : Ok(result);
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout(LogoutRequestDto dto, CancellationToken cancellationToken)
    {
        await authService.LogoutAsync(dto, cancellationToken);
        return NoContent();
    }
}
