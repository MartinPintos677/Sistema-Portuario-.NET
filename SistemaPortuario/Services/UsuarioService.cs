using Microsoft.EntityFrameworkCore;
using SistemaPortuario.Data;
using SistemaPortuario.DTOs;
using SistemaPortuario.Models;

namespace SistemaPortuario.Services;

/// <summary>
/// Servicio para usuarios y roles.
/// Gestiona altas, ediciones, cambios de password y activacion de cuentas.
/// </summary>
public class UsuarioService(SistemaPortuarioDbContext context, ICurrentUserService currentUser) : IUsuarioService
{
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

        entity.IdEmpresa = dto.IdEmpresa;
        entity.IdRol = dto.IdRol;
        entity.Cedula = dto.Cedula;
        entity.Nombre = dto.Nombre;
        entity.Apellido = dto.Apellido;
        entity.Correo = dto.Correo;
        entity.Telefono = dto.Telefono;
        entity.Activo = dto.Activo;
        await context.SaveChangesAsync(cancellationToken);
        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<bool> SetActivoAsync(int id, bool activo, CancellationToken cancellationToken = default)
    {
        var entity = await UsuariosEditables().FirstOrDefaultAsync(u => u.IdUsuario == id, cancellationToken);
        if (entity is null)
        {
            return false;
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
            throw new ArgumentException("La empresa indicada no existe o esta inactiva.");
        }
    }
}
