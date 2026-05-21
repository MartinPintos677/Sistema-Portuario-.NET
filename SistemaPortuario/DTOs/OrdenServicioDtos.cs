using System.ComponentModel.DataAnnotations;

namespace SistemaPortuario.DTOs;

// DTOs de ordenes de servicio, estados, documentos y facturacion.
public record EstadoOrdenServicioResponseDto(
    int IdEstadoOrden,
    string Nombre,
    string? Descripcion);

public record OrdenServicioResponseDto(
    int IdOrdenServicio,
    int IdEmpresa,
    string Empresa,
    int IdCliente,
    string Cliente,
    int IdEncargado,
    string Encargado,
    int IdOperario,
    string Operario,
    int IdMaquinariaAsignada,
    string MaquinariaAsignada,
    int? IdMaquinariaFacturada,
    string? MaquinariaFacturada,
    int IdEstadoOrden,
    string EstadoOrden,
    DateTime FechaSolicitud,
    string LugarServicio,
    string TrabajoARealizar,
    DateTime? HoraInicioEstimada,
    DateTime? HoraInicioReal,
    DateTime? HoraFinalizacion,
    string? Observaciones,
    bool RequiereFirmaCliente,
    bool EnviadaCliente,
    bool PrecargadaGSoft);

public record OrdenServicioCreateDto(
    [Range(1, int.MaxValue)]
    int IdEmpresa,
    [Range(1, int.MaxValue)]
    int IdCliente,
    [Range(1, int.MaxValue)]
    int IdEncargado,
    [Range(1, int.MaxValue)]
    int IdOperario,
    [Range(1, int.MaxValue)]
    int IdMaquinariaAsignada,
    [Range(1, int.MaxValue)]
    int? IdMaquinariaFacturada,
    [Range(1, int.MaxValue)]
    int IdEstadoOrden,
    [Required, StringLength(250)]
    string LugarServicio,
    [Required, StringLength(1000)]
    string TrabajoARealizar,
    DateTime? HoraInicioEstimada,
    [StringLength(1000)]
    string? Observaciones,
    bool RequiereFirmaCliente);

public record OrdenServicioUpdateDto(
    [Range(1, int.MaxValue)]
    int IdCliente,
    [Range(1, int.MaxValue)]
    int IdEncargado,
    [Range(1, int.MaxValue)]
    int IdOperario,
    [Range(1, int.MaxValue)]
    int IdMaquinariaAsignada,
    [Range(1, int.MaxValue)]
    int? IdMaquinariaFacturada,
    [Range(1, int.MaxValue)]
    int IdEstadoOrden,
    [Required, StringLength(250)]
    string LugarServicio,
    [Required, StringLength(1000)]
    string TrabajoARealizar,
    DateTime? HoraInicioEstimada,
    DateTime? HoraInicioReal,
    DateTime? HoraFinalizacion,
    [StringLength(1000)]
    string? Observaciones,
    bool RequiereFirmaCliente,
    bool EnviadaCliente,
    bool PrecargadaGSoft);

public record FinalizarOrdenServicioDto(
    DateTime HoraFinalizacion,
    [StringLength(1000)]
    string? Observaciones);

public record DocumentoPdfResponseDto(
    int IdDocumento,
    int IdOrdenServicio,
    string NombreArchivo,
    string RutaArchivo,
    DateTime FechaGeneracion);

public record FacturacionResponseDto(
    int IdFacturacion,
    int IdOrdenServicio,
    DateTime? FechaEnvio,
    string Estado,
    string? ReferenciaGSoft,
    string? Observaciones);

public record FacturacionCreateDto(
    [Range(1, int.MaxValue)]
    int IdOrdenServicio,
    DateTime? FechaEnvio,
    [Required, StringLength(80)]
    string Estado,
    [StringLength(120)]
    string? ReferenciaGSoft,
    [StringLength(1000)]
    string? Observaciones);
