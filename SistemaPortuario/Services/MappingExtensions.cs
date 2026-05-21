using SistemaPortuario.DTOs;
using SistemaPortuario.Models;

namespace SistemaPortuario.Services;

/// <summary>
/// Mapeos entre entidades EF Core y DTOs de respuesta.
/// Mantiene las proyecciones simples fuera de controllers y services.
/// </summary>
public static class MappingExtensions
{
    public static EmpresaResponseDto ToDto(this Empresa entity) =>
        new(entity.IdEmpresa, entity.Nombre, entity.RazonSocial, entity.Rut, entity.TipoEmpresa, entity.Activa);

    public static RolResponseDto ToDto(this Rol entity) =>
        new(entity.IdRol, entity.Nombre);

    public static UsuarioResponseDto ToDto(this Usuario entity) =>
        new(
            entity.IdUsuario,
            entity.IdEmpresa,
            entity.Empresa?.Nombre ?? string.Empty,
            entity.IdRol,
            entity.Rol?.Nombre ?? string.Empty,
            entity.Cedula,
            entity.Nombre,
            entity.Apellido,
            entity.Correo,
            entity.Telefono,
            entity.Activo);

    public static ClienteResponseDto ToDto(this Cliente entity) =>
        new(
            entity.IdCliente,
            entity.IdEmpresa,
            entity.Empresa?.Nombre ?? string.Empty,
            entity.RazonSocial,
            entity.Rut,
            entity.NombreContacto,
            entity.Correo,
            entity.Telefono,
            entity.Direccion,
            entity.Activo);

    public static TipoMaquinariaResponseDto ToDto(this TipoMaquinaria entity) =>
        new(entity.IdTipoMaquinaria, entity.Nombre);

    public static MaquinariaResponseDto ToDto(this Maquinaria entity) =>
        new(
            entity.IdMaquinaria,
            entity.IdEmpresa,
            entity.Empresa?.Nombre ?? string.Empty,
            entity.IdTipoMaquinaria,
            entity.TipoMaquinaria?.Nombre ?? string.Empty,
            entity.Codigo,
            entity.Nombre,
            entity.Marca,
            entity.Modelo,
            entity.Matricula,
            entity.HorasAcumuladas,
            entity.Activa);

    public static RegistroHorasMaquinariaResponseDto ToDto(this RegistroHorasMaquinaria entity) =>
        new(
            entity.IdRegistroHoras,
            entity.IdMaquinaria,
            entity.Maquinaria?.Nombre ?? string.Empty,
            entity.IdOrdenServicio,
            entity.Fecha,
            entity.HorasTrabajadas,
            entity.Observacion);

    public static EstadoOrdenServicioResponseDto ToDto(this EstadoOrdenServicio entity) =>
        new(entity.IdEstadoOrden, entity.Nombre, entity.Descripcion);

    public static OrdenServicioResponseDto ToDto(this OrdenServicio entity) =>
        new(
            entity.IdOrdenServicio,
            entity.IdEmpresa,
            entity.Empresa?.Nombre ?? string.Empty,
            entity.IdCliente,
            entity.Cliente?.RazonSocial ?? string.Empty,
            entity.IdEncargado,
            entity.Encargado?.NombreCompleto() ?? string.Empty,
            entity.IdOperario,
            entity.Operario?.NombreCompleto() ?? string.Empty,
            entity.IdMaquinariaAsignada,
            entity.MaquinariaAsignada?.Nombre ?? string.Empty,
            entity.IdMaquinariaFacturada,
            entity.MaquinariaFacturada?.Nombre,
            entity.IdEstadoOrden,
            entity.EstadoOrden?.Nombre ?? string.Empty,
            entity.FechaSolicitud,
            entity.LugarServicio,
            entity.TrabajoARealizar,
            entity.HoraInicioEstimada,
            entity.HoraInicioReal,
            entity.HoraFinalizacion,
            entity.Observaciones,
            entity.RequiereFirmaCliente,
            entity.EnviadaCliente,
            entity.PrecargadaGSoft);

    public static DocumentoPdfResponseDto ToDto(this DocumentoPdf entity) =>
        new(entity.IdDocumento, entity.IdOrdenServicio, entity.NombreArchivo, entity.RutaArchivo, entity.FechaGeneracion);

    public static FacturacionResponseDto ToDto(this Facturacion entity) =>
        new(entity.IdFacturacion, entity.IdOrdenServicio, entity.FechaEnvio, entity.Estado, entity.ReferenciaGSoft, entity.Observaciones);

    public static TipoMantenimientoResponseDto ToDto(this TipoMantenimiento entity) =>
        new(entity.IdTipoMantenimiento, entity.Nombre, entity.Descripcion, entity.UmbralHoras);

    public static EstadoMantenimientoResponseDto ToDto(this EstadoMantenimiento entity) =>
        new(entity.IdEstadoMantenimiento, entity.Nombre);

    public static MantenimientoResponseDto ToDto(this Mantenimiento entity) =>
        new(
            entity.IdMantenimiento,
            entity.IdMaquinaria,
            entity.Maquinaria?.Nombre ?? string.Empty,
            entity.IdTipoMantenimiento,
            entity.TipoMantenimiento?.Nombre ?? string.Empty,
            entity.IdEstadoMantenimiento,
            entity.EstadoMantenimiento?.Nombre ?? string.Empty,
            entity.IdResponsable,
            entity.Responsable?.NombreCompleto(),
            entity.IdRegistroHorasOrigen,
            entity.FechaProgramada,
            entity.FechaRealizada,
            entity.Descripcion,
            entity.HorasMaquinaAlMomento,
            entity.Observaciones);

    public static EstadoTareaResponseDto ToDto(this EstadoTarea entity) =>
        new(entity.IdEstadoTarea, entity.Nombre);

    public static TareaAdministrativaResponseDto ToDto(this TareaAdministrativa entity) =>
        new(
            entity.IdTarea,
            entity.IdCreador,
            entity.Creador?.NombreCompleto() ?? string.Empty,
            entity.IdAsignado,
            entity.Asignado?.NombreCompleto() ?? string.Empty,
            entity.IdEstadoTarea,
            entity.EstadoTarea?.Nombre ?? string.Empty,
            entity.Titulo,
            entity.Descripcion,
            entity.FechaCreacion,
            entity.FechaVencimiento,
            entity.Prioridad);

    public static EventoCalendarioResponseDto ToDto(this EventoCalendario entity) =>
        new(
            entity.IdEvento,
            entity.IdCreador,
            entity.Creador?.NombreCompleto() ?? string.Empty,
            entity.IdTarea,
            entity.Tarea?.Titulo,
            entity.Titulo,
            entity.Descripcion,
            entity.FechaInicio,
            entity.FechaFin,
            entity.TipoEvento);

    public static PersonalEstibaResponseDto ToDto(this PersonalEstiba entity) =>
        new(
            entity.IdPersonalEstiba,
            entity.IdEmpresa,
            entity.Empresa?.Nombre ?? string.Empty,
            entity.Cedula,
            entity.Nombre,
            entity.Apellido,
            entity.Telefono,
            entity.Activo);

    public static CuadrillaResponseDto ToDto(this Cuadrilla entity) =>
        new(
            entity.IdCuadrilla,
            entity.IdEmpresa,
            entity.Empresa?.Nombre ?? string.Empty,
            entity.Nombre,
            entity.Descripcion,
            entity.Activa);

    public static CuadrillaPersonalResponseDto ToDto(this CuadrillaPersonal entity) =>
        new(
            entity.IdCuadrillaPersonal,
            entity.IdCuadrilla,
            entity.Cuadrilla?.Nombre ?? string.Empty,
            entity.IdPersonalEstiba,
            entity.PersonalEstiba?.NombreCompleto() ?? string.Empty,
            entity.FechaDesde,
            entity.FechaHasta);

    public static EstadoCitacionResponseDto ToDto(this EstadoCitacion entity) =>
        new(entity.IdEstadoCitacion, entity.Nombre);

    public static CitacionEstibaResponseDto ToDto(this CitacionEstiba entity) =>
        new(
            entity.IdCitacion,
            entity.IdEmpresa,
            entity.Empresa?.Nombre ?? string.Empty,
            entity.IdCliente,
            entity.Cliente?.RazonSocial,
            entity.IdEstadoCitacion,
            entity.EstadoCitacion?.Nombre ?? string.Empty,
            entity.Fecha,
            entity.Hora,
            entity.Zona,
            entity.DetalleOperativo);

    public static DetalleCitacionEstibaResponseDto ToDto(this DetalleCitacionEstiba entity) =>
        new(
            entity.IdDetalleCitacion,
            entity.IdCitacion,
            entity.IdPersonalEstiba,
            entity.PersonalEstiba?.NombreCompleto() ?? string.Empty,
            entity.IdCuadrilla,
            entity.Cuadrilla?.Nombre,
            entity.IdLiquidacion,
            entity.Asistencia,
            entity.HoraInicioReal,
            entity.HoraFinReal,
            entity.HorasTrabajadas,
            entity.EstadoAltaBps,
            entity.Observaciones);

    public static LiquidacionEstibaResponseDto ToDto(this LiquidacionEstiba entity) =>
        new(
            entity.IdLiquidacion,
            entity.IdEmpresa,
            entity.Empresa?.Nombre ?? string.Empty,
            entity.PeriodoDesde,
            entity.PeriodoHasta,
            entity.TotalHoras,
            entity.Estado,
            entity.FechaGeneracion,
            entity.Observaciones);

    public static NotificacionResponseDto ToDto(this Notificacion entity) =>
        new(entity.IdNotificacion, entity.IdUsuario, entity.IdOrdenServicio, entity.IdCitacion, entity.Tipo, entity.Destinatario, entity.Mensaje, entity.FechaEnvio, entity.Estado, entity.FechaCreacion);

    public static TrazabilidadResponseDto ToDto(this Trazabilidad entity) =>
        new(
            entity.IdTrazabilidad,
            entity.IdUsuario,
            entity.Usuario?.NombreCompleto() ?? string.Empty,
            entity.Fecha,
            entity.Accion,
            entity.Entidad,
            entity.IdRegistroAfectado,
            entity.DatosPrevios,
            entity.DatosNuevos);

    private static string NombreCompleto(this Usuario usuario) =>
        $"{usuario.Nombre} {usuario.Apellido}".Trim();

    private static string NombreCompleto(this PersonalEstiba personal) =>
        $"{personal.Nombre} {personal.Apellido}".Trim();
}
