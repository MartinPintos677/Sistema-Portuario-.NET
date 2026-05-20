namespace SistemaPortuario.Models;

public class Cliente
{
    public int IdCliente { get; set; }
    public int IdEmpresa { get; set; }
    public string RazonSocial { get; set; } = string.Empty;
    public string Rut { get; set; } = string.Empty;
    public string? NombreContacto { get; set; }
    public string? Correo { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; }

    public Empresa Empresa { get; set; } = null!;
    public ICollection<OrdenServicio> OrdenesServicio { get; set; } = new List<OrdenServicio>();
    public ICollection<CitacionEstiba> CitacionesEstiba { get; set; } = new List<CitacionEstiba>();
}
