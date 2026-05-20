using System.ComponentModel.DataAnnotations;

namespace SistemaPortuario.DTOs;

public record ClienteResponseDto(
    int IdCliente,
    int IdEmpresa,
    string Empresa,
    string RazonSocial,
    string Rut,
    string? NombreContacto,
    string? Correo,
    string? Telefono,
    string? Direccion,
    bool Activo);

public record ClienteCreateDto(
    [Range(1, int.MaxValue)]
    int IdEmpresa,
    [Required, StringLength(160)]
    string RazonSocial,
    [Required, StringLength(30)]
    string Rut,
    [StringLength(140)]
    string? NombreContacto,
    [EmailAddress, StringLength(180)]
    string? Correo,
    [StringLength(50)]
    string? Telefono,
    [StringLength(250)]
    string? Direccion);

public record ClienteUpdateDto(
    [Range(1, int.MaxValue)]
    int IdEmpresa,
    [Required, StringLength(160)]
    string RazonSocial,
    [Required, StringLength(30)]
    string Rut,
    [StringLength(140)]
    string? NombreContacto,
    [EmailAddress, StringLength(180)]
    string? Correo,
    [StringLength(50)]
    string? Telefono,
    [StringLength(250)]
    string? Direccion,
    bool Activo);
