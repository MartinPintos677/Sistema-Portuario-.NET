using System.ComponentModel.DataAnnotations;

namespace SistemaPortuario.DTOs;

// DTOs de empresas, usuarios y roles usados por administracion y permisos.
public record EmpresaResponseDto(
    int IdEmpresa,
    string Nombre,
    string RazonSocial,
    string Rut,
    string TipoEmpresa,
    bool Activa);

public record EmpresaCreateDto(
    [Required, StringLength(120)]
    string Nombre,
    [Required, StringLength(160)]
    string RazonSocial,
    [Required, StringLength(30)]
    string Rut,
    [Required, StringLength(80)]
    string TipoEmpresa);

public record EmpresaUpdateDto(
    [Required, StringLength(120)]
    string Nombre,
    [Required, StringLength(160)]
    string RazonSocial,
    [Required, StringLength(30)]
    string Rut,
    [Required, StringLength(80)]
    string TipoEmpresa,
    bool Activa);

public record RolResponseDto(
    int IdRol,
    string Nombre);

public record UsuarioResponseDto(
    int IdUsuario,
    int IdEmpresa,
    string Empresa,
    int IdRol,
    string Rol,
    string Cedula,
    string Nombre,
    string Apellido,
    string Correo,
    string? Telefono,
    bool Activo);

public record UsuarioCreateDto(
    [Range(1, int.MaxValue)]
    int IdEmpresa,
    [Range(1, int.MaxValue)]
    int IdRol,
    [Required, StringLength(30)]
    string Cedula,
    [Required, StringLength(100)]
    string Nombre,
    [Required, StringLength(100)]
    string Apellido,
    [Required, EmailAddress, StringLength(180)]
    string Correo,
    [StringLength(50)]
    string? Telefono,
    [Required, StringLength(100, MinimumLength = 6)]
    string Password);

public record UsuarioUpdateDto(
    [Range(1, int.MaxValue)]
    int IdEmpresa,
    [Range(1, int.MaxValue)]
    int IdRol,
    [Required, StringLength(30)]
    string Cedula,
    [Required, StringLength(100)]
    string Nombre,
    [Required, StringLength(100)]
    string Apellido,
    [Required, EmailAddress, StringLength(180)]
    string Correo,
    [StringLength(50)]
    string? Telefono,
    bool Activo);
