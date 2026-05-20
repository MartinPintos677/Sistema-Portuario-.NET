using Microsoft.EntityFrameworkCore;
using SistemaPortuario.Data;
using SistemaPortuario.DTOs;
using SistemaPortuario.Models;
using SistemaPortuario.Security;

namespace SistemaPortuario.Services;

public class NotificacionService(SistemaPortuarioDbContext context, ICurrentUserService currentUser) : INotificacionService
{
    public async Task<PagedResponseDto<NotificacionResponseDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default) =>
        await NotificacionesScoped()
            .AsNoTracking()
            .OrderByDescending(n => n.FechaCreacion)
            .Select(n => n.ToDto())
            .ToPagedResponseAsync(pagination, cancellationToken);

    public async Task<NotificacionResponseDto> CreateAsync(NotificacionCreateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureNotificationScopeAsync(dto.IdUsuario, dto.IdOrdenServicio, dto.IdCitacion, cancellationToken);

        var entity = new Notificacion
        {
            IdUsuario = dto.IdUsuario,
            IdOrdenServicio = dto.IdOrdenServicio,
            IdCitacion = dto.IdCitacion,
            Tipo = dto.Tipo,
            Destinatario = dto.Destinatario,
            Mensaje = dto.Mensaje,
            Estado = "Pendiente"
        };

        context.Notificaciones.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }

    public async Task<NotificacionResponseDto?> UpdateEstadoAsync(int id, NotificacionUpdateEstadoDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await NotificacionesScoped().FirstOrDefaultAsync(n => n.IdNotificacion == id, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        entity.Estado = dto.Estado;
        entity.FechaEnvio = dto.FechaEnvio;
        await context.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }

    private IQueryable<Notificacion> NotificacionesScoped()
    {
        var query = context.Notificaciones
            .Include(n => n.Usuario)
            .Include(n => n.OrdenServicio)
            .Include(n => n.Citacion);

        return currentUser.Rol == AppRoles.Operario && currentUser.IdUsuario.HasValue
            ? query.Where(n =>
                n.IdUsuario == currentUser.IdUsuario.Value ||
                (n.OrdenServicio != null && n.OrdenServicio.IdOperario == currentUser.IdUsuario.Value))
            : currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
                ? query
                : query.Where(n =>
                    (n.Usuario != null && n.Usuario.IdEmpresa == currentUser.IdEmpresa.Value) ||
                    (n.OrdenServicio != null && n.OrdenServicio.IdEmpresa == currentUser.IdEmpresa.Value) ||
                    (n.Citacion != null && n.Citacion.IdEmpresa == currentUser.IdEmpresa.Value));
    }

    private async Task EnsureNotificationScopeAsync(
        int? idUsuario,
        int? idOrdenServicio,
        int? idCitacion,
        CancellationToken cancellationToken)
    {
        var empresas = new List<int>();

        if (idUsuario.HasValue)
        {
            var idEmpresa = await context.Usuarios
                .Where(u => u.IdUsuario == idUsuario.Value)
                .Select(u => (int?)u.IdEmpresa)
                .FirstOrDefaultAsync(cancellationToken);

            if (idEmpresa is null)
            {
                throw new ArgumentException("El usuario indicado no existe.");
            }

            empresas.Add(idEmpresa.Value);
        }

        if (idOrdenServicio.HasValue)
        {
            var idEmpresa = await context.OrdenesServicio
                .Where(o => o.IdOrdenServicio == idOrdenServicio.Value)
                .Select(o => (int?)o.IdEmpresa)
                .FirstOrDefaultAsync(cancellationToken);

            if (idEmpresa is null)
            {
                throw new ArgumentException("La orden indicada no existe.");
            }

            empresas.Add(idEmpresa.Value);
        }

        if (idCitacion.HasValue)
        {
            var idEmpresa = await context.CitacionesEstiba
                .Where(c => c.IdCitacion == idCitacion.Value)
                .Select(c => (int?)c.IdEmpresa)
                .FirstOrDefaultAsync(cancellationToken);

            if (idEmpresa is null)
            {
                throw new ArgumentException("La citacion indicada no existe.");
            }

            empresas.Add(idEmpresa.Value);
        }

        if (empresas.Count == 0)
        {
            throw new ArgumentException("La notificacion debe estar asociada a un usuario, orden o citacion.");
        }

        if (empresas.Distinct().Count() > 1 || empresas.Any(idEmpresa => !currentUser.CanAccessEmpresa(idEmpresa)))
        {
            throw new ArgumentException("La notificacion solo puede asociar datos de una empresa permitida.");
        }
    }
}
