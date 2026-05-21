namespace SistemaPortuario.Models;

/// <summary>
/// Refresh token persistido como hash para renovar sesiones de forma segura.
/// </summary>
public class RefreshToken
{
    public int IdRefreshToken { get; set; }
    public int IdUsuario { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public DateTime Expira { get; set; }
    public DateTime? FechaRevocacion { get; set; }
    public string? ReemplazadoPorTokenHash { get; set; }

    public Usuario Usuario { get; set; } = null!;

    public bool Activo => FechaRevocacion is null && Expira > DateTime.UtcNow;
}
