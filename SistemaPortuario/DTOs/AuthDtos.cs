using System.ComponentModel.DataAnnotations;

namespace SistemaPortuario.DTOs;

public record LoginRequestDto(
    [Required, EmailAddress, StringLength(180)]
    string Correo,
    [Required, StringLength(100, MinimumLength = 6)]
    string Password);

public record LoginResponseDto(
    string Token,
    string RefreshToken,
    DateTime Expira,
    UsuarioResponseDto Usuario);

public record RefreshTokenRequestDto(
    [Required]
    string RefreshToken);

public record LogoutRequestDto(
    string? RefreshToken);

public record PrimerAdministradorRequestDto(
    [Required, StringLength(120)]
    string EmpresaNombre,
    [Required, StringLength(160)]
    string EmpresaRazonSocial,
    [Required, StringLength(30)]
    string EmpresaRut,
    [Required, StringLength(80)]
    string EmpresaTipo,
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

public record RegisterUsuarioRequestDto(
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

public record CambiarPasswordRequestDto(
    [Required, StringLength(100, MinimumLength = 6)]
    string PasswordActual,
    [Required, StringLength(100, MinimumLength = 6)]
    string PasswordNueva);
