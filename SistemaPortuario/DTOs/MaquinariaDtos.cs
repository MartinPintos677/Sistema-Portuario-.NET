using System.ComponentModel.DataAnnotations;

namespace SistemaPortuario.DTOs;

// DTOs de maquinaria, tipos y registros de horas.
public record TipoMaquinariaResponseDto(
    int IdTipoMaquinaria,
    string Nombre);

public record TipoMaquinariaCreateDto(
    [Required, StringLength(80)]
    string Nombre);

public record MaquinariaResponseDto(
    int IdMaquinaria,
    int IdEmpresa,
    string Empresa,
    int IdTipoMaquinaria,
    string TipoMaquinaria,
    string Codigo,
    string Nombre,
    string? Marca,
    string? Modelo,
    string? Matricula,
    decimal HorasAcumuladas,
    bool Activa);

public record MaquinariaCreateDto(
    [Range(1, int.MaxValue)]
    int IdEmpresa,
    [Range(1, int.MaxValue)]
    int IdTipoMaquinaria,
    [Required, StringLength(60)]
    string Codigo,
    [Required, StringLength(120)]
    string Nombre,
    [StringLength(100)]
    string? Marca,
    [StringLength(100)]
    string? Modelo,
    [StringLength(80)]
    string? Matricula);

public record MaquinariaUpdateDto(
    [Range(1, int.MaxValue)]
    int IdEmpresa,
    [Range(1, int.MaxValue)]
    int IdTipoMaquinaria,
    [Required, StringLength(60)]
    string Codigo,
    [Required, StringLength(120)]
    string Nombre,
    [StringLength(100)]
    string? Marca,
    [StringLength(100)]
    string? Modelo,
    [StringLength(80)]
    string? Matricula,
    [Range(typeof(decimal), "0", "9999999999")]
    decimal HorasAcumuladas,
    bool Activa);

public record RegistroHorasMaquinariaResponseDto(
    int IdRegistroHoras,
    int IdMaquinaria,
    string Maquinaria,
    int? IdOrdenServicio,
    DateOnly Fecha,
    decimal HorasTrabajadas,
    string? Observacion);

public record RegistroHorasMaquinariaCreateDto(
    [Range(1, int.MaxValue)]
    int IdMaquinaria,
    [Range(1, int.MaxValue)]
    int? IdOrdenServicio,
    DateOnly Fecha,
    [Range(typeof(decimal), "0.01", "9999999999")]
    decimal HorasTrabajadas,
    [StringLength(500)]
    string? Observacion);
