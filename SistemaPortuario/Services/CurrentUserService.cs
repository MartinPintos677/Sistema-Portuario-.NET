using System.Security.Claims;
using SistemaPortuario.Security;

namespace SistemaPortuario.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public int? IdUsuario => GetIntClaim(ClaimTypes.NameIdentifier);

    public int? IdEmpresa => GetIntClaim("empresa_id");

    public string? Rol => User?.FindFirstValue(ClaimTypes.Role);

    public bool IsAdministrador => Rol == AppRoles.Administrador;

    public bool CanAccessEmpresa(int idEmpresa) => IsAdministrador || IdEmpresa == idEmpresa;

    private int? GetIntClaim(string claimType)
    {
        var value = User?.FindFirstValue(claimType);
        return int.TryParse(value, out var result) ? result : null;
    }
}
