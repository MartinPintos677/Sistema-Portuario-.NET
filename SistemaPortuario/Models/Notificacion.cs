namespace SistemaPortuario.Models;

/// <summary>
/// Mensaje generado por el sistema para usuario, orden o citacion.
/// </summary>
public class Notificacion
{
    public int IdNotificacion { get; set; }
    public int? IdUsuario { get; set; }
    public int? IdOrdenServicio { get; set; }
    public int? IdCitacion { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Destinatario { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public DateTime? FechaEnvio { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }

    public Usuario? Usuario { get; set; }
    public OrdenServicio? OrdenServicio { get; set; }
    public CitacionEstiba? Citacion { get; set; }
}
