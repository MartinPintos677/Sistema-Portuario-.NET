namespace SistemaPortuario.Models;

/// <summary>
/// Catálogo de estados para tareas administrativas.
/// </summary>
public class EstadoTarea
{
    public int IdEstadoTarea { get; set; }
    public string Nombre { get; set; } = string.Empty;

    public ICollection<TareaAdministrativa> Tareas { get; set; } = new List<TareaAdministrativa>();
}

public class TareaAdministrativa
{
    public int IdTarea { get; set; }
    public int IdCreador { get; set; }
    public int IdAsignado { get; set; }
    public int IdEstadoTarea { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public string Prioridad { get; set; } = "Media";

    public Usuario Creador { get; set; } = null!;
    public Usuario Asignado { get; set; } = null!;
    public EstadoTarea EstadoTarea { get; set; } = null!;
    public ICollection<EventoCalendario> Eventos { get; set; } = new List<EventoCalendario>();
}

public class EventoCalendario
{
    public int IdEvento { get; set; }
    public int IdCreador { get; set; }
    public int? IdTarea { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string TipoEvento { get; set; } = string.Empty;

    public Usuario Creador { get; set; } = null!;
    public TareaAdministrativa? Tarea { get; set; }
}

