using Microsoft.EntityFrameworkCore;
using SistemaPortuario.Data;
using SistemaPortuario.DTOs;
using SistemaPortuario.Models;
using SistemaPortuario.Security;

namespace SistemaPortuario.Services;

/// <summary>
/// Servicio para tareas administrativas y eventos de calendario.
/// Aplica filtros de empresa y mapea entidades a DTOs de uso del frontend.
/// </summary>
public class AdministracionService(SistemaPortuarioDbContext context, ICurrentUserService currentUser) : IAdministracionService
{
    public async Task<List<EstadoTareaResponseDto>> GetEstadosTareaAsync(CancellationToken cancellationToken = default) =>
        await context.EstadosTarea.AsNoTracking().OrderBy(e => e.IdEstadoTarea).Select(e => e.ToDto()).ToListAsync(cancellationToken);

    public async Task<PagedResponseDto<TareaAdministrativaResponseDto>> GetTareasAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default) =>
        await TareasConRelaciones()
            .OrderByDescending(t => t.FechaCreacion)
            .Select(t => t.ToDto())
            .ToPagedResponseAsync(pagination, cancellationToken);

    public async Task<TareaAdministrativaResponseDto?> GetTareaByIdAsync(int id, CancellationToken cancellationToken = default) =>
        (await TareasConRelaciones().FirstOrDefaultAsync(t => t.IdTarea == id, cancellationToken))?.ToDto();

    public async Task<TareaAdministrativaResponseDto> CreateTareaAsync(int idCreador, TareaAdministrativaCreateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureUsuarioEmpresaAsync(idCreador, cancellationToken);
        await EnsureAsignacionPermitidaAsync(dto.IdAsignado, cancellationToken);

        var entity = new TareaAdministrativa
        {
            IdCreador = idCreador,
            IdAsignado = dto.IdAsignado,
            IdEstadoTarea = dto.IdEstadoTarea,
            Titulo = dto.Titulo,
            Descripcion = dto.Descripcion,
            FechaVencimiento = dto.FechaVencimiento,
            Prioridad = dto.Prioridad
        };

        context.TareasAdministrativas.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return (await GetTareaByIdAsync(entity.IdTarea, cancellationToken))!;
    }

    public async Task<TareaAdministrativaResponseDto?> UpdateTareaAsync(int id, TareaAdministrativaUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await context.TareasAdministrativas.FirstOrDefaultAsync(t => t.IdTarea == id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        await EnsureAsignacionPermitidaAsync(dto.IdAsignado, cancellationToken);

        entity.IdAsignado = dto.IdAsignado;
        entity.IdEstadoTarea = dto.IdEstadoTarea;
        entity.Titulo = dto.Titulo;
        entity.Descripcion = dto.Descripcion;
        entity.FechaVencimiento = dto.FechaVencimiento;
        entity.Prioridad = dto.Prioridad;
        await context.SaveChangesAsync(cancellationToken);
        return await GetTareaByIdAsync(id, cancellationToken);
    }

    public async Task<PagedResponseDto<EventoCalendarioResponseDto>> GetEventosAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default) =>
        await EventosConRelaciones()
            .OrderBy(e => e.FechaInicio)
            .Select(e => e.ToDto())
            .ToPagedResponseAsync(pagination, cancellationToken);

    public async Task<EventoCalendarioResponseDto> CreateEventoAsync(int idCreador, EventoCalendarioCreateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureUsuarioEmpresaAsync(idCreador, cancellationToken);

        if (dto.IdTarea.HasValue)
        {
            var tareaValida = await TareasConRelaciones().AnyAsync(t => t.IdTarea == dto.IdTarea.Value, cancellationToken);
            if (!tareaValida)
            {
                throw new ArgumentException("La tarea indicada no existe.");
            }
        }

        var entity = new EventoCalendario
        {
            IdCreador = idCreador,
            IdTarea = dto.IdTarea,
            Titulo = dto.Titulo,
            Descripcion = dto.Descripcion,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            TipoEvento = dto.TipoEvento
        };

        context.EventosCalendario.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return (await EventosConRelaciones()
            .FirstAsync(e => e.IdEvento == entity.IdEvento, cancellationToken))
            .ToDto();
    }

    private IQueryable<TareaAdministrativa> TareasConRelaciones() =>
        TareasPermitidas()
            .AsNoTracking()
            .Include(t => t.Creador)
            .Include(t => t.Asignado)
            .Include(t => t.EstadoTarea);

    private IQueryable<EventoCalendario> EventosConRelaciones() =>
        EventosPermitidos()
            .AsNoTracking()
            .Include(e => e.Creador)
            .Include(e => e.Tarea);

    private IQueryable<TareaAdministrativa> TareasPermitidas()
    {
        var query = context.TareasAdministrativas.AsQueryable();
        return currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
            ? query
            : query.Where(t =>
                t.Creador.IdEmpresa == currentUser.IdEmpresa.Value ||
                t.Asignado.IdEmpresa == currentUser.IdEmpresa.Value);
    }

    private IQueryable<EventoCalendario> EventosPermitidos()
    {
        var query = context.EventosCalendario.AsQueryable();
        return currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
            ? query
            : query.Where(e => e.Creador.IdEmpresa == currentUser.IdEmpresa.Value);
    }

    private async Task EnsureUsuarioEmpresaAsync(int idUsuario, CancellationToken cancellationToken)
    {
        var idEmpresa = await context.Usuarios
            .Where(u => u.IdUsuario == idUsuario)
            .Select(u => (int?)u.IdEmpresa)
            .FirstOrDefaultAsync(cancellationToken);

        if (idEmpresa is null)
        {
            throw new ArgumentException("El usuario indicado no existe.");
        }

        if (!currentUser.CanAccessEmpresa(idEmpresa.Value))
        {
            throw new UnauthorizedAccessException("No tienes permisos para operar con esa empresa.");
        }
    }

    private async Task EnsureAsignacionPermitidaAsync(int idAsignado, CancellationToken cancellationToken)
    {
        var usuario = await context.Usuarios
            .AsNoTracking()
            .Include(u => u.Rol)
            .Where(u => u.IdUsuario == idAsignado)
            .Select(u => new { u.IdEmpresa, Rol = u.Rol.Nombre })
            .FirstOrDefaultAsync(cancellationToken);

        if (usuario is null)
        {
            throw new ArgumentException("El usuario asignado no existe.");
        }

        if (!currentUser.CanAccessEmpresa(usuario.IdEmpresa))
        {
            throw new UnauthorizedAccessException("No tienes permisos para asignar tareas a usuarios de esa empresa.");
        }

        var rolesPermitidos = currentUser.Rol switch
        {
            AppRoles.Administrador => new[] { AppRoles.Administrador, AppRoles.Oficina, AppRoles.Encargado, AppRoles.Operario },
            AppRoles.Oficina => new[] { AppRoles.Administrador, AppRoles.Oficina, AppRoles.Encargado },
            AppRoles.Encargado => new[] { AppRoles.Oficina, AppRoles.Encargado, AppRoles.Operario },
            _ => Array.Empty<string>()
        };

        if (!rolesPermitidos.Contains(usuario.Rol))
        {
            throw new UnauthorizedAccessException("No puedes asignar tareas a ese rol.");
        }
    }
}
