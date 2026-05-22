using Microsoft.EntityFrameworkCore;
using SistemaPortuario.Data;
using SistemaPortuario.DTOs;
using SistemaPortuario.Models;

namespace SistemaPortuario.Services;

/// <summary>
/// Servicio para usuarios y roles.
/// Gestiona altas, ediciónes, cambios de password y activacíon de cuentas.
/// </summary>
public class UsuarioService(SistemaPortuarioDbContext context, ICurrentUserService currentUser) : IUsuarioService
{
    private const string AdminDemoCorreo = "admin.demo@sistema-portuario.local";

    public async Task<List<RolResponseDto>> GetRolesAsync(CancellationToken cancellationToken = default) =>
        await context.Roles.AsNoTracking().OrderBy(r => r.Nombre).Select(r => r.ToDto()).ToListAsync(cancellationToken);

    public async Task<PagedResponseDto<UsuarioResponseDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default) =>
        await UsuariosConRelaciones()
            .OrderBy(u => u.Apellido)
            .ThenBy(u => u.Nombre)
            .Select(u => u.ToDto())
            .ToPagedResponseAsync(pagination, cancellationToken);

    public async Task<UsuarioResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        (await UsuariosConRelaciones().FirstOrDefaultAsync(u => u.IdUsuario == id, cancellationToken))?.ToDto();

    public async Task<UsuarioResponseDto?> GetPerfilAsync(CancellationToken cancellationToken = default)
    {
        if (!currentUser.IdUsuario.HasValue)
        {
            return null;
        }

        return (await UsuariosConRelaciones()
            .FirstOrDefaultAsync(u => u.IdUsuario == currentUser.IdUsuario.Value && u.Activo, cancellationToken))
            ?.ToDto();
    }

    public async Task<UsuarioResponseDto> CreateAsync(UsuarioCreateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureEmpresaPermitidaAsync(dto.IdEmpresa, cancellationToken);

        var entity = new Usuario
        {
            IdEmpresa = dto.IdEmpresa,
            IdRol = dto.IdRol,
            Cedula = dto.Cedula,
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Correo = dto.Correo,
            Telefono = dto.Telefono,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        context.Usuarios.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return (await GetByIdAsync(entity.IdUsuario, cancellationToken))!;
    }

    public async Task<UsuarioResponseDto?> UpdateAsync(int id, UsuarioUpdateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureEmpresaPermitidaAsync(dto.IdEmpresa, cancellationToken);

        var entity = await UsuariosEditables().FirstOrDefaultAsync(u => u.IdUsuario == id, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        var isAdminDemo = IsAdminDemo(entity);
        entity.IdEmpresa = dto.IdEmpresa;
        entity.IdRol = dto.IdRol;
        entity.Cedula = dto.Cedula;
        entity.Nombre = dto.Nombre;
        entity.Apellido = dto.Apellido;
        entity.Correo = dto.Correo;
        entity.Telefono = dto.Telefono;
        entity.Activo = dto.Activo;
        if (isAdminDemo)
        {
            entity.Activo = true;
        }

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        }

        await context.SaveChangesAsync(cancellationToken);
        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<UsuarioResponseDto?> UpdatePerfilAsync(UsuarioPerfilUpdateDto dto, CancellationToken cancellationToken = default)
    {
        if (!currentUser.IdUsuario.HasValue)
        {
            return null;
        }

        var entity = await context.Usuarios
            .FirstOrDefaultAsync(u => u.IdUsuario == currentUser.IdUsuario.Value && u.Activo, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        entity.Telefono = dto.Telefono;
        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        }

        await context.SaveChangesAsync(cancellationToken);
        return await GetPerfilAsync(cancellationToken);
    }

    public async Task<bool> SetActivoAsync(int id, bool activo, CancellationToken cancellationToken = default)
    {
        var entity = await UsuariosEditables().FirstOrDefaultAsync(u => u.IdUsuario == id, cancellationToken);
        if (entity is null)
        {
            return false;
        }

        if (!activo && IsAdminDemo(entity))
        {
            throw new ArgumentException("El administrador demo principal no puede desactivarse.");
        }

        entity.Activo = activo;
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private IQueryable<Usuario> UsuariosConRelaciones() =>
        UsuariosPermitidos()
            .AsNoTracking()
            .Include(u => u.Empresa)
            .Include(u => u.Rol);

    private IQueryable<Usuario> UsuariosPermitidos()
    {
        var query = context.Usuarios.AsQueryable();
        return currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
            ? query
            : query.Where(u => u.IdEmpresa == currentUser.IdEmpresa.Value);
    }

    private IQueryable<Usuario> UsuariosEditables()
    {
        var query = context.Usuarios.AsQueryable();
        return currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
            ? query
            : query.Where(u => u.IdEmpresa == currentUser.IdEmpresa.Value);
    }

    private async Task EnsureEmpresaPermitidaAsync(int idEmpresa, CancellationToken cancellationToken)
    {
        if (!currentUser.CanAccessEmpresa(idEmpresa))
        {
            throw new UnauthorizedAccessException("No tienes permisos para operar con esa empresa.");
        }

        var existe = await context.Empresas.AnyAsync(e => e.IdEmpresa == idEmpresa && e.Activa, cancellationToken);
        if (!existe)
        {
            throw new ArgumentException("La empresa indicada no existe o está inactiva.");
        }
    }

    private static bool IsAdminDemo(Usuario usuario) =>
        string.Equals(usuario.Correo, AdminDemoCorreo, StringComparison.OrdinalIgnoreCase);
}

