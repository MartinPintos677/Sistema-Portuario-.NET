namespace SistemaPortuario.Models;

/// <summary>
/// Personal operativo utilizado en citaciones de estiba.
/// </summary>
public class PersonalEstiba
{
    public int IdPersonalEstiba { get; set; }
    public int IdEmpresa { get; set; }
    public string Cedula { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; }

    public Empresa Empresa { get; set; } = null!;
    public ICollection<CuadrillaPersonal> CuadrillasPersonal { get; set; } = new List<CuadrillaPersonal>();
    public ICollection<DetalleCitacionEstiba> DetallesCitacion { get; set; } = new List<DetalleCitacionEstiba>();
}

public class Cuadrilla
{
    public int IdCuadrilla { get; set; }
    public int IdEmpresa { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activa { get; set; } = true;

    public Empresa Empresa { get; set; } = null!;
    public ICollection<CuadrillaPersonal> Personal { get; set; } = new List<CuadrillaPersonal>();
    public ICollection<DetalleCitacionEstiba> DetallesCitacion { get; set; } = new List<DetalleCitacionEstiba>();
}

public class CuadrillaPersonal
{
    public int IdCuadrillaPersonal { get; set; }
    public int IdCuadrilla { get; set; }
    public int IdPersonalEstiba { get; set; }
    public DateOnly FechaDesde { get; set; }
    public DateOnly? FechaHasta { get; set; }

    public Cuadrilla Cuadrilla { get; set; } = null!;
    public PersonalEstiba PersonalEstiba { get; set; } = null!;
}

public class EstadoCitacion
{
    public int IdEstadoCitacion { get; set; }
    public string Nombre { get; set; } = string.Empty;

    public ICollection<CitacionEstiba> Citaciones { get; set; } = new List<CitacionEstiba>();
}

public class CitacionEstiba
{
    public int IdCitacion { get; set; }
    public int IdEmpresa { get; set; }
    public int? IdCliente { get; set; }
    public int IdEstadoCitacion { get; set; }
    public DateOnly Fecha { get; set; }
    public TimeOnly Hora { get; set; }
    public string Zona { get; set; } = string.Empty;
    public string? DetalleOperativo { get; set; }
    public DateTime FechaCreacion { get; set; }

    public Empresa Empresa { get; set; } = null!;
    public Cliente? Cliente { get; set; }
    public EstadoCitacion EstadoCitacion { get; set; } = null!;
    public ICollection<DetalleCitacionEstiba> Detalles { get; set; } = new List<DetalleCitacionEstiba>();
    public ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
}

public class LiquidacionEstiba
{
    public int IdLiquidacion { get; set; }
    public int IdEmpresa { get; set; }
    public DateOnly PeriodoDesde { get; set; }
    public DateOnly PeriodoHasta { get; set; }
    public decimal TotalHoras { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaGeneracion { get; set; }
    public string? Observaciones { get; set; }

    public Empresa Empresa { get; set; } = null!;
    public ICollection<DetalleCitacionEstiba> DetallesCitacion { get; set; } = new List<DetalleCitacionEstiba>();
}

public class DetalleCitacionEstiba
{
    public int IdDetalleCitacion { get; set; }
    public int IdCitacion { get; set; }
    public int IdPersonalEstiba { get; set; }
    public int? IdCuadrilla { get; set; }
    public int? IdLiquidacion { get; set; }
    public bool? Asistencia { get; set; }
    public TimeOnly? HoraInicioReal { get; set; }
    public TimeOnly? HoraFinReal { get; set; }
    public decimal? HorasTrabajadas { get; set; }
    public string? EstadoAltaBps { get; set; }
    public string? Observaciones { get; set; }

    public CitacionEstiba Citacion { get; set; } = null!;
    public PersonalEstiba PersonalEstiba { get; set; } = null!;
    public Cuadrilla? Cuadrilla { get; set; }
    public LiquidacionEstiba? Liquidacion { get; set; }
}
