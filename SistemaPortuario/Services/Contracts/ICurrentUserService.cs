namespace SistemaPortuario.Services;

/// <summary>
/// Contrato para acceder al usuario autenticado desde los services.
/// </summary>
public interface ICurrentUserService
{
    int? IdUsuario { get; }
    int? IdEmpresa { get; }
    string? Rol { get; }
    bool IsAdministrador { get; }
    bool CanAccessEmpresa(int idEmpresa);
}
