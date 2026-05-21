using System.ComponentModel.DataAnnotations;

namespace SistemaPortuario.DTOs;

// DTOs de estiba: personal, cuadrillas, citaciones, detalles y liquidaciones.
public record PersonalEstibaResponseDto(
    int IdPersonalEstiba,
    int IdEmpresa,
    string Empresa,
    string Cedula,
    string Nombre,
    string Apellido,
    string? Telefono,
    bool Activo);

public record PersonalEstibaCreateDto(
    [Range(1, int.MaxValue)]
    int IdEmpresa,
    [Required, StringLength(30)]
    string Cedula,
    [Required, StringLength(100)]
    string Nombre,
    [Required, StringLength(100)]
    string Apellido,
    [StringLength(50)]
    string? Telefono);

public record PersonalEstibaUpdateDto(
    [Range(1, int.MaxValue)]
    int IdEmpresa,
    [Required, StringLength(30)]
    string Cedula,
    [Required, StringLength(100)]
    string Nombre,
    [Required, StringLength(100)]
    string Apellido,
    [StringLength(50)]
    string? Telefono,
    bool Activo);

public record CuadrillaResponseDto(
    int IdCuadrilla,
    int IdEmpresa,
    string Empresa,
    string Nombre,
    string? Descripcion,
    bool Activa);

public record CuadrillaCreateDto(
    [Range(1, int.MaxValue)]
    int IdEmpresa,
    [Required, StringLength(100)]
    string Nombre,
    [StringLength(500)]
    string? Descripcion);

public record CuadrillaUpdateDto(
    [Range(1, int.MaxValue)]
    int IdEmpresa,
    [Required, StringLength(100)]
    string Nombre,
    [StringLength(500)]
    string? Descripcion,
    bool Activa);

public record CuadrillaPersonalResponseDto(
    int IdCuadrillaPersonal,
    int IdCuadrilla,
    string Cuadrilla,
    int IdPersonalEstiba,
    string PersonalEstiba,
    DateOnly FechaDesde,
    DateOnly? FechaHasta);

public record CuadrillaPersonalCreateDto(
    [Range(1, int.MaxValue)]
    int IdCuadrilla,
    [Range(1, int.MaxValue)]
    int IdPersonalEstiba,
    DateOnly FechaDesde,
    DateOnly? FechaHasta);

public record EstadoCitacionResponseDto(
    int IdEstadoCitacion,
    string Nombre);

public record CitacionEstibaResponseDto(
    int IdCitacion,
    int IdEmpresa,
    string Empresa,
    int? IdCliente,
    string? Cliente,
    int IdEstadoCitacion,
    string EstadoCitacion,
    DateOnly Fecha,
    TimeOnly Hora,
    string Zona,
    string? DetalleOperativo);

public record CitacionEstibaCreateDto(
    [Range(1, int.MaxValue)]
    int IdEmpresa,
    [Range(1, int.MaxValue)]
    int? IdCliente,
    [Range(1, int.MaxValue)]
    int IdEstadoCitacion,
    DateOnly Fecha,
    TimeOnly Hora,
    [Required, StringLength(120)]
    string Zona,
    [StringLength(1000)]
    string? DetalleOperativo);

public record CitacionEstibaUpdateDto(
    [Range(1, int.MaxValue)]
    int IdEmpresa,
    [Range(1, int.MaxValue)]
    int? IdCliente,
    [Range(1, int.MaxValue)]
    int IdEstadoCitacion,
    DateOnly Fecha,
    TimeOnly Hora,
    [Required, StringLength(120)]
    string Zona,
    [StringLength(1000)]
    string? DetalleOperativo);

public record DetalleCitacionEstibaResponseDto(
    int IdDetalleCitacion,
    int IdCitacion,
    int IdPersonalEstiba,
    string PersonalEstiba,
    int? IdCuadrilla,
    string? Cuadrilla,
    int? IdLiquidacion,
    bool? Asistencia,
    TimeOnly? HoraInicioReal,
    TimeOnly? HoraFinReal,
    decimal? HorasTrabajadas,
    string? EstadoAltaBps,
    string? Observaciones);

public record DetalleCitacionEstibaCreateDto(
    [Range(1, int.MaxValue)]
    int IdCitacion,
    [Range(1, int.MaxValue)]
    int IdPersonalEstiba,
    [Range(1, int.MaxValue)]
    int? IdCuadrilla,
    bool? Asistencia,
    TimeOnly? HoraInicioReal,
    TimeOnly? HoraFinReal,
    [Range(typeof(decimal), "0", "9999999999")]
    decimal? HorasTrabajadas,
    [StringLength(80)]
    string? EstadoAltaBps,
    [StringLength(1000)]
    string? Observaciones);

public record DetalleCitacionEstibaUpdateDto(
    [Range(1, int.MaxValue)]
    int? IdCuadrilla,
    [Range(1, int.MaxValue)]
    int? IdLiquidacion,
    bool? Asistencia,
    TimeOnly? HoraInicioReal,
    TimeOnly? HoraFinReal,
    [Range(typeof(decimal), "0", "9999999999")]
    decimal? HorasTrabajadas,
    [StringLength(80)]
    string? EstadoAltaBps,
    [StringLength(1000)]
    string? Observaciones);

public record LiquidacionEstibaResponseDto(
    int IdLiquidacion,
    int IdEmpresa,
    string Empresa,
    DateOnly PeriodoDesde,
    DateOnly PeriodoHasta,
    decimal TotalHoras,
    string Estado,
    DateTime FechaGeneracion,
    string? Observaciones);

public record LiquidacionEstibaCreateDto(
    [Range(1, int.MaxValue)]
    int IdEmpresa,
    DateOnly PeriodoDesde,
    DateOnly PeriodoHasta,
    [Required, StringLength(80)]
    string Estado,
    [StringLength(1000)]
    string? Observaciones);

public record LiquidacionEstibaUpdateDto(
    [Range(1, int.MaxValue)]
    int IdEmpresa,
    DateOnly PeriodoDesde,
    DateOnly PeriodoHasta,
    [Range(typeof(decimal), "0", "9999999999")]
    decimal TotalHoras,
    [Required, StringLength(80)]
    string Estado,
    [StringLength(1000)]
    string? Observaciones);
