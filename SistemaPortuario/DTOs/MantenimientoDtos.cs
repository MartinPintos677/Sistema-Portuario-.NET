using System.ComponentModel.DataAnnotations;

namespace SistemaPortuario.DTOs;

// DTOs de mantenimiento de maquinaria y sus catálogos.
public record TipoMantenimientoResponseDto(
    int IdTipoMantenimiento,
    string Nombre,
    string? Descripcion,
    decimal? UmbralHoras);

public record TipoMantenimientoCreateDto(
    [Required, StringLength(100)]
    string Nombre,
    [StringLength(500)]
    string? Descripcion,
    [Range(typeof(decimal), "0.01", "9999999999")]
    decimal? UmbralHoras);

public record EstadoMantenimientoResponseDto(
    int IdEstadoMantenimiento,
    string Nombre);

public record MantenimientoResponseDto(
    int IdMantenimiento,
    int IdMaquinaria,
    string Maquinaria,
    int IdTipoMantenimiento,
    string TipoMantenimiento,
    int IdEstadoMantenimiento,
    string EstadoMantenimiento,
    int? IdResponsable,
    string? Responsable,
    int? IdRegistroHorasOrigen,
    DateOnly? FechaProgramada,
    DateOnly? FechaRealizada,
    string Descripcion,
    decimal? HorasMaquinaAlMomento,
    string? Observaciones);

public record MantenimientoCreateDto(
    [Range(1, int.MaxValue)]
    int IdMaquinaria,
    [Range(1, int.MaxValue)]
    int IdTipoMantenimiento,
    [Range(1, int.MaxValue)]
    int IdEstadoMantenimiento,
    [Range(1, int.MaxValue)]
    int? IdResponsable,
    [Range(1, int.MaxValue)]
    int? IdRegistroHorasOrigen,
    DateOnly? FechaProgramada,
    [Required, StringLength(1000)]
    string Descripcion,
    [Range(typeof(decimal), "0", "9999999999")]
    decimal? HorasMaquinaAlMomento,
    [StringLength(1000)]
    string? Observaciones);

public record MantenimientoUpdateDto(
    [Range(1, int.MaxValue)]
    int IdTipoMantenimiento,
    [Range(1, int.MaxValue)]
    int IdEstadoMantenimiento,
    [Range(1, int.MaxValue)]
    int? IdResponsable,
    [Range(1, int.MaxValue)]
    int? IdRegistroHorasOrigen,
    DateOnly? FechaProgramada,
    DateOnly? FechaRealizada,
    [Required, StringLength(1000)]
    string Descripcion,
    [Range(typeof(decimal), "0", "9999999999")]
    decimal? HorasMaquinaAlMomento,
    [StringLength(1000)]
    string? Observaciones);

