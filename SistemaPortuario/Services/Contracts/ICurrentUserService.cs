namespace SistemaPortuario.Services;

public interface ICurrentUserService
{
    int? IdUsuario { get; }
    int? IdEmpresa { get; }
    string? Rol { get; }
    bool IsAdministrador { get; }
    bool CanAccessEmpresa(int idEmpresa);
}
