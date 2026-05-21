namespace SistemaPortuario.Models;

/// <summary>
/// Empresa operadora o vinculada al sistema.
/// Actua como limite de datos para la mayoria de los roles.
/// </summary>
public class Empresa
{
    public int IdEmpresa { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public string Rut { get; set; } = string.Empty;
    public string TipoEmpresa { get; set; } = string.Empty;
    public bool Activa { get; set; } = true;
    public DateTime FechaCreacion { get; set; }

    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
    public ICollection<Maquinaria> Maquinarias { get; set; } = new List<Maquinaria>();
    public ICollection<OrdenServicio> OrdenesServicio { get; set; } = new List<OrdenServicio>();
    public ICollection<PersonalEstiba> PersonalEstiba { get; set; } = new List<PersonalEstiba>();
    public ICollection<Cuadrilla> Cuadrillas { get; set; } = new List<Cuadrilla>();
    public ICollection<CitacionEstiba> CitacionesEstiba { get; set; } = new List<CitacionEstiba>();
    public ICollection<LiquidacionEstiba> LiquidacionesEstiba { get; set; } = new List<LiquidacionEstiba>();
}
