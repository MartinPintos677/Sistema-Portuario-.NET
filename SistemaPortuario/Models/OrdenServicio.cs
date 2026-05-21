namespace SistemaPortuario.Models;

/// <summary>
/// Catalogo de estados posibles para una orden de servicio.
/// </summary>
public class EstadoOrdenServicio
{
    public int IdEstadoOrden { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public ICollection<OrdenServicio> OrdenesServicio { get; set; } = new List<OrdenServicio>();
}

/// <summary>
/// Trabajo operativo solicitado para cliente, operario y maquinaria asignada.
/// </summary>
public class OrdenServicio
{
    public int IdOrdenServicio { get; set; }
    public int IdEmpresa { get; set; }
    public int IdCliente { get; set; }
    public int IdEncargado { get; set; }
    public int IdOperario { get; set; }
    public int IdMaquinariaAsignada { get; set; }
    public int? IdMaquinariaFacturada { get; set; }
    public int IdEstadoOrden { get; set; }
    public DateTime FechaSolicitud { get; set; }
    public string LugarServicio { get; set; } = string.Empty;
    public string TrabajoARealizar { get; set; } = string.Empty;
    public DateTime? HoraInicioEstimada { get; set; }
    public DateTime? HoraInicioReal { get; set; }
    public DateTime? HoraFinalizacion { get; set; }
    public string? Observaciones { get; set; }
    public bool RequiereFirmaCliente { get; set; }
    public bool EnviadaCliente { get; set; }
    public bool PrecargadaGSoft { get; set; }
    public DateTime FechaCreacion { get; set; }

    public Empresa Empresa { get; set; } = null!;
    public Cliente Cliente { get; set; } = null!;
    public Usuario Encargado { get; set; } = null!;
    public Usuario Operario { get; set; } = null!;
    public Maquinaria MaquinariaAsignada { get; set; } = null!;
    public Maquinaria? MaquinariaFacturada { get; set; }
    public EstadoOrdenServicio EstadoOrden { get; set; } = null!;
    public DocumentoPdf? DocumentoPdf { get; set; }
    public Facturacion? Facturacion { get; set; }
    public ICollection<RegistroHorasMaquinaria> RegistrosHorasMaquinaria { get; set; } = new List<RegistroHorasMaquinaria>();
    public ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
}

public class DocumentoPdf
{
    public int IdDocumento { get; set; }
    public int IdOrdenServicio { get; set; }
    public string NombreArchivo { get; set; } = string.Empty;
    public string RutaArchivo { get; set; } = string.Empty;
    public DateTime FechaGeneracion { get; set; }

    public OrdenServicio OrdenServicio { get; set; } = null!;
}

public class Facturacion
{
    public int IdFacturacion { get; set; }
    public int IdOrdenServicio { get; set; }
    public DateTime? FechaEnvio { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? ReferenciaGSoft { get; set; }
    public string? Observaciones { get; set; }

    public OrdenServicio OrdenServicio { get; set; } = null!;
}
