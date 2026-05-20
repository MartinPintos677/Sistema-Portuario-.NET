namespace SistemaPortuario.Security;

public static class AppRoles
{
    public const string Administrador = "Administrador";
    public const string Encargado = "Encargado";
    public const string Operario = "Operario";
    public const string Oficina = "Oficina";

    public const string Administracion = $"{Administrador},{Oficina}";
    public const string GestionOperativa = $"{Administrador},{Encargado},{Oficina}";
    public const string OrdenesLectura = $"{Administrador},{Encargado},{Operario},{Oficina}";
    public const string Estiba = $"{Administrador},{Encargado},{Oficina}";
    public const string Taller = $"{Administrador},{Encargado},{Oficina}";
}
