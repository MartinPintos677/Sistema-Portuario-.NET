using System.ComponentModel.DataAnnotations;

namespace SistemaPortuario.DTOs;

public record EstadoTareaResponseDto(
    int IdEstadoTarea,
    string Nombre);

public record TareaAdministrativaResponseDto(
    int IdTarea,
    int IdCreador,
    string Creador,
    int IdAsignado,
    string Asignado,
    int IdEstadoTarea,
    string EstadoTarea,
    string Titulo,
    string? Descripcion,
    DateTime FechaCreacion,
    DateTime? FechaVencimiento,
    string Prioridad);

public record TareaAdministrativaCreateDto(
    [Range(1, int.MaxValue)]
    int IdAsignado,
    [Range(1, int.MaxValue)]
    int IdEstadoTarea,
    [Required, StringLength(160)]
    string Titulo,
    [StringLength(1000)]
    string? Descripcion,
    DateTime? FechaVencimiento,
    [Required, StringLength(30)]
    string Prioridad);

public record TareaAdministrativaUpdateDto(
    [Range(1, int.MaxValue)]
    int IdAsignado,
    [Range(1, int.MaxValue)]
    int IdEstadoTarea,
    [Required, StringLength(160)]
    string Titulo,
    [StringLength(1000)]
    string? Descripcion,
    DateTime? FechaVencimiento,
    [Required, StringLength(30)]
    string Prioridad);

public record EventoCalendarioResponseDto(
    int IdEvento,
    int IdCreador,
    string Creador,
    int? IdTarea,
    string? Tarea,
    string Titulo,
    string? Descripcion,
    DateTime FechaInicio,
    DateTime? FechaFin,
    string TipoEvento);

public record EventoCalendarioCreateDto(
    [Range(1, int.MaxValue)]
    int? IdTarea,
    [Required, StringLength(160)]
    string Titulo,
    [StringLength(1000)]
    string? Descripcion,
    DateTime FechaInicio,
    DateTime? FechaFin,
    [Required, StringLength(80)]
    string TipoEvento);

public record EventoCalendarioUpdateDto(
    [Range(1, int.MaxValue)]
    int? IdTarea,
    [Required, StringLength(160)]
    string Titulo,
    [StringLength(1000)]
    string? Descripcion,
    DateTime FechaInicio,
    DateTime? FechaFin,
    [Required, StringLength(80)]
    string TipoEvento);
