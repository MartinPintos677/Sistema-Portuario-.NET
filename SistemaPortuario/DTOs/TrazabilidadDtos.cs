namespace SistemaPortuario.DTOs;

// DTOs de auditoría expuestos al frontend de trazabilidad.
public record TrazabilidadResponseDto(
    long IdTrazabilidad,
    int IdUsuario,
    string Usuario,
    DateTime Fecha,
    string Accion,
    string Entidad,
    string IdRegistroAfectado,
    string? DatosPrevios,
    string? DatosNuevos);

