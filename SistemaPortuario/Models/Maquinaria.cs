namespace SistemaPortuario.Models;

public class TipoMaquinaria
{
    public int IdTipoMaquinaria { get; set; }
    public string Nombre { get; set; } = string.Empty;

    public ICollection<Maquinaria> Maquinarias { get; set; } = new List<Maquinaria>();
}

public class Maquinaria
{
    public int IdMaquinaria { get; set; }
    public int IdEmpresa { get; set; }
    public int IdTipoMaquinaria { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public string? Matricula { get; set; }
    public decimal HorasAcumuladas { get; set; }
    public bool Activa { get; set; } = true;
    public DateTime FechaCreacion { get; set; }

    public Empresa Empresa { get; set; } = null!;
    public TipoMaquinaria TipoMaquinaria { get; set; } = null!;
    public ICollection<OrdenServicio> OrdenesAsignadas { get; set; } = new List<OrdenServicio>();
    public ICollection<OrdenServicio> OrdenesFacturadas { get; set; } = new List<OrdenServicio>();
    public ICollection<RegistroHorasMaquinaria> RegistrosHoras { get; set; } = new List<RegistroHorasMaquinaria>();
    public ICollection<Mantenimiento> Mantenimientos { get; set; } = new List<Mantenimiento>();
}

public class RegistroHorasMaquinaria
{
    public int IdRegistroHoras { get; set; }
    public int IdMaquinaria { get; set; }
    public int? IdOrdenServicio { get; set; }
    public DateOnly Fecha { get; set; }
    public decimal HorasTrabajadas { get; set; }
    public string? Observacion { get; set; }
    public DateTime FechaCreacion { get; set; }

    public Maquinaria Maquinaria { get; set; } = null!;
    public OrdenServicio? OrdenServicio { get; set; }
    public ICollection<Mantenimiento> MantenimientosOriginados { get; set; } = new List<Mantenimiento>();
}
