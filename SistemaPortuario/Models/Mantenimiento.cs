namespace SistemaPortuario.Models;

/// <summary>
/// Catalogo de tipos de mantenimiento aplicables a maquinaria.
/// </summary>
public class TipoMantenimiento
{
    public int IdTipoMantenimiento { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal? UmbralHoras { get; set; }

    public ICollection<Mantenimiento> Mantenimientos { get; set; } = new List<Mantenimiento>();
}

public class EstadoMantenimiento
{
    public int IdEstadoMantenimiento { get; set; }
    public string Nombre { get; set; } = string.Empty;

    public ICollection<Mantenimiento> Mantenimientos { get; set; } = new List<Mantenimiento>();
}

public class Mantenimiento
{
    public int IdMantenimiento { get; set; }
    public int IdMaquinaria { get; set; }
    public int IdTipoMantenimiento { get; set; }
    public int IdEstadoMantenimiento { get; set; }
    public int? IdResponsable { get; set; }
    public int? IdRegistroHorasOrigen { get; set; }
    public DateOnly? FechaProgramada { get; set; }
    public DateOnly? FechaRealizada { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal? HorasMaquinaAlMomento { get; set; }
    public string? Observaciones { get; set; }
    public DateTime FechaCreacion { get; set; }

    public Maquinaria Maquinaria { get; set; } = null!;
    public TipoMantenimiento TipoMantenimiento { get; set; } = null!;
    public EstadoMantenimiento EstadoMantenimiento { get; set; } = null!;
    public Usuario? Responsable { get; set; }
    public RegistroHorasMaquinaria? RegistroHorasOrigen { get; set; }
}
