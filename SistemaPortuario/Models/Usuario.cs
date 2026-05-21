namespace SistemaPortuario.Models;

/// <summary>
/// Usuario del sistema con rol, empresa y relaciones operativas.
/// </summary>
public class Usuario
{
    public int IdUsuario { get; set; }
    public int IdEmpresa { get; set; }
    public int IdRol { get; set; }
    public string Cedula { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; }

    public Empresa Empresa { get; set; } = null!;
    public Rol Rol { get; set; } = null!;
    public ICollection<OrdenServicio> OrdenesComoEncargado { get; set; } = new List<OrdenServicio>();
    public ICollection<OrdenServicio> OrdenesComoOperario { get; set; } = new List<OrdenServicio>();
    public ICollection<Mantenimiento> MantenimientosResponsable { get; set; } = new List<Mantenimiento>();
    public ICollection<TareaAdministrativa> TareasCreadas { get; set; } = new List<TareaAdministrativa>();
    public ICollection<TareaAdministrativa> TareasAsignadas { get; set; } = new List<TareaAdministrativa>();
    public ICollection<EventoCalendario> EventosCreados { get; set; } = new List<EventoCalendario>();
    public ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
    public ICollection<Trazabilidad> Trazabilidades { get; set; } = new List<Trazabilidad>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
