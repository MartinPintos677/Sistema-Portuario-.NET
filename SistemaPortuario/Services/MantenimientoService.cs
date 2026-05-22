using Microsoft.EntityFrameworkCore;
using SistemaPortuario.Data;
using SistemaPortuario.DTOs;
using SistemaPortuario.Models;

namespace SistemaPortuario.Services;

/// <summary>
/// Servicio para mantenimiento de maquinaria.
/// Gestiona catálogos, órdenes de mantenimiento y cierre de tareas técnicas.
/// </summary>
public class MantenimientoService(SistemaPortuarioDbContext context, ICurrentUserService currentUser) : IMantenimientoService
{
    public async Task<List<TipoMantenimientoResponseDto>> GetTiposAsync(CancellationToken cancellationToken = default) =>
        await context.TiposMantenimiento.AsNoTracking().OrderBy(t => t.Nombre).Select(t => t.ToDto()).ToListAsync(cancellationToken);

    public async Task<TipoMantenimientoResponseDto> CreateTipoAsync(TipoMantenimientoCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new TipoMantenimiento
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            UmbralHoras = dto.UmbralHoras
        };

        context.TiposMantenimiento.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }

    public async Task<List<EstadoMantenimientoResponseDto>> GetEstadosAsync(CancellationToken cancellationToken = default) =>
        await context.EstadosMantenimiento.AsNoTracking().OrderBy(e => e.IdEstadoMantenimiento).Select(e => e.ToDto()).ToListAsync(cancellationToken);

    public async Task<PagedResponseDto<MantenimientoResponseDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default) =>
        await MantenimientosConRelaciones()
            .OrderByDescending(m => m.FechaProgramada)
            .Select(m => m.ToDto())
            .ToPagedResponseAsync(pagination, cancellationToken);

    public async Task<MantenimientoResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        (await MantenimientosConRelaciones().FirstOrDefaultAsync(m => m.IdMantenimiento == id, cancellationToken))?.ToDto();

    public async Task<MantenimientoResponseDto> CreateAsync(MantenimientoCreateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureMantenimientoPayloadAsync(dto.IdMaquinaria, dto.IdResponsable, dto.IdRegistroHorasOrigen, cancellationToken);

        var entity = new Mantenimiento
        {
            IdMaquinaria = dto.IdMaquinaria,
            IdTipoMantenimiento = dto.IdTipoMantenimiento,
            IdEstadoMantenimiento = dto.IdEstadoMantenimiento,
            IdResponsable = dto.IdResponsable,
            IdRegistroHorasOrigen = dto.IdRegistroHorasOrigen,
            FechaProgramada = dto.FechaProgramada,
            Descripcion = dto.Descripcion,
            HorasMaquinaAlMomento = dto.HorasMaquinaAlMomento,
            Observaciones = dto.Observaciones
        };

        context.Mantenimientos.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return (await GetByIdAsync(entity.IdMantenimiento, cancellationToken))!;
    }

    public async Task<MantenimientoResponseDto?> UpdateAsync(int id, MantenimientoUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await MantenimientosEditables()
            .Include(m => m.Maquinaria)
            .FirstOrDefaultAsync(m => m.IdMantenimiento == id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        await EnsureMantenimientoPayloadAsync(entity.IdMaquinaria, dto.IdResponsable, dto.IdRegistroHorasOrigen, cancellationToken);

        entity.IdTipoMantenimiento = dto.IdTipoMantenimiento;
        entity.IdEstadoMantenimiento = dto.IdEstadoMantenimiento;
        entity.IdResponsable = dto.IdResponsable;
        entity.IdRegistroHorasOrigen = dto.IdRegistroHorasOrigen;
        entity.FechaProgramada = dto.FechaProgramada;
        entity.FechaRealizada = dto.FechaRealizada;
        entity.Descripcion = dto.Descripcion;
        entity.HorasMaquinaAlMomento = dto.HorasMaquinaAlMomento;
        entity.Observaciones = dto.Observaciones;

        await context.SaveChangesAsync(cancellationToken);
        return await GetByIdAsync(id, cancellationToken);
    }

    private IQueryable<Mantenimiento> MantenimientosConRelaciones() =>
        MantenimientosPermitidos()
            .AsNoTracking()
            .Include(m => m.Maquinaria)
            .Include(m => m.TipoMantenimiento)
            .Include(m => m.EstadoMantenimiento)
            .Include(m => m.Responsable);

    private IQueryable<Mantenimiento> MantenimientosPermitidos()
    {
        var query = context.Mantenimientos.AsQueryable();
        return currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
            ? query
            : query.Where(m => m.Maquinaria.IdEmpresa == currentUser.IdEmpresa.Value);
    }

    private IQueryable<Mantenimiento> MantenimientosEditables()
    {
        var query = context.Mantenimientos.AsQueryable();
        return currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
            ? query
            : query.Where(m => m.Maquinaria.IdEmpresa == currentUser.IdEmpresa.Value);
    }

    private async Task EnsureMantenimientoPayloadAsync(
        int idMaquinaria,
        int? idResponsable,
        int? idRegistroHorasOrigen,
        CancellationToken cancellationToken)
    {
        var idEmpresa = await context.Maquinarias
            .Where(m => m.IdMaquinaria == idMaquinaria)
            .Select(m => (int?)m.IdEmpresa)
            .FirstOrDefaultAsync(cancellationToken);

        if (idEmpresa is null)
        {
            throw new ArgumentException("La maquinaria indicada no existe.");
        }

        if (!currentUser.CanAccessEmpresa(idEmpresa.Value))
        {
            throw new UnauthorizedAccessException("No tienes permisos para operar con esa empresa.");
        }

        if (idResponsable.HasValue)
        {
            var responsableValido = await context.Usuarios.AnyAsync(
                u => u.IdUsuario == idResponsable.Value && u.IdEmpresa == idEmpresa,
                cancellationToken);

            if (!responsableValido)
            {
                throw new ArgumentException("El responsable no pertenece a la empresa de la maquinaria.");
            }
        }

        if (idRegistroHorasOrigen.HasValue)
        {
            var registroValido = await context.RegistrosHorasMaquinaria.AnyAsync(
                r => r.IdRegistroHoras == idRegistroHorasOrigen.Value && r.Maquinaria.IdEmpresa == idEmpresa,
                cancellationToken);

            if (!registroValido)
            {
                throw new ArgumentException("El registro de horas no pertenece a la empresa de la maquinaria.");
            }
        }
    }
}

