namespace SistemaPortuario.Models;

/// <summary>
/// Registro de auditoría generado automáticamente al guardar cambios.
/// </summary>
public class Trazabilidad
{
    public long IdTrazabilidad { get; set; }
    public int IdUsuario { get; set; }
    public DateTime Fecha { get; set; }
    public string Accion { get; set; } = string.Empty;
    public string Entidad { get; set; } = string.Empty;
    public string IdRegistroAfectado { get; set; } = string.Empty;
    public string? DatosPrevios { get; set; }
    public string? DatosNuevos { get; set; }

    public Usuario Usuario { get; set; } = null!;
}
