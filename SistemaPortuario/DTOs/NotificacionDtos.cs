using System.ComponentModel.DataAnnotations;

namespace SistemaPortuario.DTOs;

public record NotificacionResponseDto(
    int IdNotificacion,
    int? IdUsuario,
    int? IdOrdenServicio,
    int? IdCitacion,
    string Tipo,
    string Destinatario,
    string Mensaje,
    DateTime? FechaEnvio,
    string Estado,
    DateTime FechaCreacion);

public record NotificacionCreateDto(
    [Range(1, int.MaxValue)]
    int? IdUsuario,
    [Range(1, int.MaxValue)]
    int? IdOrdenServicio,
    [Range(1, int.MaxValue)]
    int? IdCitacion,
    [Required, StringLength(40)]
    string Tipo,
    [Required, StringLength(180)]
    string Destinatario,
    [Required, StringLength(1000)]
    string Mensaje);

public record NotificacionUpdateEstadoDto(
    [Required, StringLength(80)]
    string Estado,
    DateTime? FechaEnvio);
