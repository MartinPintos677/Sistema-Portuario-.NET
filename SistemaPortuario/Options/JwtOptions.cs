namespace SistemaPortuario.Options;

/// <summary>
/// Opciones de configuracion usadas para emitir y validar tokens JWT.
/// Se cargan desde la seccion Jwt de appsettings o variables de entorno.
/// </summary>
public class JwtOptions
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; } = 480;
}
