using SistemaPortuario.DTOs;

namespace SistemaPortuario.Services;

/// <summary>
/// Contrato de autenticacion y ciclo de sesion.
/// </summary>
public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default);
    Task<LoginResponseDto?> CrearPrimerAdministradorAsync(PrimerAdministradorRequestDto dto, CancellationToken cancellationToken = default);
    Task<LoginResponseDto?> RefreshAsync(RefreshTokenRequestDto dto, CancellationToken cancellationToken = default);
    Task LogoutAsync(LogoutRequestDto dto, CancellationToken cancellationToken = default);
}
