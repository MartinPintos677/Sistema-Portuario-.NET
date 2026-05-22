using Microsoft.EntityFrameworkCore;
using SistemaPortuario.Data;
using SistemaPortuario.DTOs;
using SistemaPortuario.Models;

namespace SistemaPortuario.Services;

/// <summary>
/// Servicio para operativa de estiba.
/// Agrupa personal, cuadrillas, citaciones, asistencia y liquidaciones.
/// </summary>
public class EstibaService(SistemaPortuarioDbContext context, ICurrentUserService currentUser) : IEstibaService
{
    public async Task<PagedResponseDto<PersonalEstibaResponseDto>> GetPersonalAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default) =>
        await PersonalPermitido()
            .AsNoTracking()
            .Include(p => p.Empresa)
            .OrderBy(p => p.Apellido)
            .ThenBy(p => p.Nombre)
            .Select(p => p.ToDto())
            .ToPagedResponseAsync(pagination, cancellationToken);

    public async Task<PersonalEstibaResponseDto> CreatePersonalAsync(PersonalEstibaCreateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureEmpresaPermitidaAsync(dto.IdEmpresa, cancellationToken);

        var entity = new PersonalEstiba
        {
            IdEmpresa = dto.IdEmpresa,
            Cedula = dto.Cedula,
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Telefono = dto.Telefono
        };

        context.PersonalEstiba.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return (await PersonalConEmpresa().FirstAsync(p => p.IdPersonalEstiba == entity.IdPersonalEstiba, cancellationToken)).ToDto();
    }

    public async Task<PersonalEstibaResponseDto?> UpdatePersonalAsync(int id, PersonalEstibaUpdateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureEmpresaPermitidaAsync(dto.IdEmpresa, cancellationToken);

        var entity = await PersonalPermitido().FirstOrDefaultAsync(p => p.IdPersonalEstiba == id, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        entity.IdEmpresa = dto.IdEmpresa;
        entity.Cedula = dto.Cedula;
        entity.Nombre = dto.Nombre;
        entity.Apellido = dto.Apellido;
        entity.Telefono = dto.Telefono;
        entity.Activo = dto.Activo;
        await context.SaveChangesAsync(cancellationToken);
        return (await PersonalConEmpresa().FirstAsync(p => p.IdPersonalEstiba == id, cancellationToken)).ToDto();
    }

    public async Task<PagedResponseDto<CuadrillaResponseDto>> GetCuadrillasAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default) =>
        await CuadrillasPermitidas()
            .AsNoTracking()
            .Include(c => c.Empresa)
            .OrderBy(c => c.Nombre)
            .Select(c => c.ToDto())
            .ToPagedResponseAsync(pagination, cancellationToken);

    public async Task<CuadrillaResponseDto> CreateCuadrillaAsync(CuadrillaCreateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureEmpresaPermitidaAsync(dto.IdEmpresa, cancellationToken);

        var entity = new Cuadrilla
        {
            IdEmpresa = dto.IdEmpresa,
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion
        };

        context.Cuadrillas.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return (await CuadrillasConEmpresa().FirstAsync(c => c.IdCuadrilla == entity.IdCuadrilla, cancellationToken)).ToDto();
    }

    public async Task<CuadrillaResponseDto?> UpdateCuadrillaAsync(int id, CuadrillaUpdateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureEmpresaPermitidaAsync(dto.IdEmpresa, cancellationToken);

        var entity = await CuadrillasPermitidas().FirstOrDefaultAsync(c => c.IdCuadrilla == id, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        entity.IdEmpresa = dto.IdEmpresa;
        entity.Nombre = dto.Nombre;
        entity.Descripcion = dto.Descripcion;
        entity.Activa = dto.Activa;
        await context.SaveChangesAsync(cancellationToken);
        return (await CuadrillasConEmpresa().FirstAsync(c => c.IdCuadrilla == id, cancellationToken)).ToDto();
    }

    public async Task<CuadrillaPersonalResponseDto> AsignarPersonalAsync(CuadrillaPersonalCreateDto dto, CancellationToken cancellationToken = default)
    {
        var cuadrilla = await CuadrillasPermitidas()
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdCuadrilla == dto.IdCuadrilla, cancellationToken);

        var personal = await PersonalPermitido()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.IdPersonalEstiba == dto.IdPersonalEstiba, cancellationToken);

        if (cuadrilla is null || personal is null)
        {
            throw new ArgumentException("La cuadrilla o el personal indicado no existe.");
        }

        if (cuadrilla.IdEmpresa != personal.IdEmpresa)
        {
            throw new ArgumentException("La cuadrilla y el personal deben pertenecer a la misma empresa.");
        }

        var entity = new CuadrillaPersonal
        {
            IdCuadrilla = dto.IdCuadrilla,
            IdPersonalEstiba = dto.IdPersonalEstiba,
            FechaDesde = dto.FechaDesde,
            FechaHasta = dto.FechaHasta
        };

        context.CuadrillasPersonal.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return (await CuadrillasPersonalConRelaciones()
            .FirstAsync(cp => cp.IdCuadrillaPersonal == entity.IdCuadrillaPersonal, cancellationToken))
            .ToDto();
    }

    public async Task<List<EstadoCitacionResponseDto>> GetEstadosCitacionAsync(CancellationToken cancellationToken = default) =>
        await context.EstadosCitacion.AsNoTracking().OrderBy(e => e.IdEstadoCitacion).Select(e => e.ToDto()).ToListAsync(cancellationToken);

    public async Task<PagedResponseDto<CitacionEstibaResponseDto>> GetCitacionesAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default) =>
        await CitacionesConRelaciones()
            .OrderByDescending(c => c.Fecha)
            .ThenBy(c => c.Hora)
            .Select(c => c.ToDto())
            .ToPagedResponseAsync(pagination, cancellationToken);

    public async Task<CitacionEstibaResponseDto> CreateCitacionAsync(CitacionEstibaCreateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureCitacionPayloadAsync(dto.IdEmpresa, dto.IdCliente, cancellationToken);

        var entity = new CitacionEstiba
        {
            IdEmpresa = dto.IdEmpresa,
            IdCliente = dto.IdCliente,
            IdEstadoCitacion = dto.IdEstadoCitacion,
            Fecha = dto.Fecha,
            Hora = dto.Hora,
            Zona = dto.Zona,
            DetalleOperativo = dto.DetalleOperativo
        };

        context.CitacionesEstiba.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return (await CitacionesConRelaciones()
            .FirstAsync(c => c.IdCitacion == entity.IdCitacion, cancellationToken))
            .ToDto();
    }

    public async Task<CitacionEstibaResponseDto?> UpdateCitacionAsync(int id, CitacionEstibaUpdateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureCitacionPayloadAsync(dto.IdEmpresa, dto.IdCliente, cancellationToken);

        var entity = await CitacionesPermitidas().FirstOrDefaultAsync(c => c.IdCitacion == id, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        entity.IdEmpresa = dto.IdEmpresa;
        entity.IdCliente = dto.IdCliente;
        entity.IdEstadoCitacion = dto.IdEstadoCitacion;
        entity.Fecha = dto.Fecha;
        entity.Hora = dto.Hora;
        entity.Zona = dto.Zona;
        entity.DetalleOperativo = dto.DetalleOperativo;
        await context.SaveChangesAsync(cancellationToken);

        return (await CitacionesConRelaciones()
            .FirstAsync(c => c.IdCitacion == id, cancellationToken))
            .ToDto();
    }

    public async Task<List<DetalleCitacionEstibaResponseDto>> GetDetallesCitacionAsync(int idCitacion, CancellationToken cancellationToken = default)
    {
        var puedeVerCitacion = await CitacionesPermitidas().AnyAsync(c => c.IdCitacion == idCitacion, cancellationToken);
        if (!puedeVerCitacion)
        {
            return [];
        }

        return await DetallesConRelaciones()
            .Where(d => d.IdCitacion == idCitacion)
            .OrderBy(d => d.PersonalEstiba.Apellido)
            .ThenBy(d => d.PersonalEstiba.Nombre)
            .Select(d => d.ToDto())
            .ToListAsync(cancellationToken);
    }

    public async Task<DetalleCitacionEstibaResponseDto> CreateDetalleCitacionAsync(DetalleCitacionEstibaCreateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureDetalleScopeAsync(dto.IdCitacion, dto.IdPersonalEstiba, dto.IdCuadrilla, null, cancellationToken);
        await EnsureDetalleNoDuplicadoAsync(dto.IdCitacion, dto.IdPersonalEstiba, cancellationToken);

        var entity = new DetalleCitacionEstiba
        {
            IdCitacion = dto.IdCitacion,
            IdPersonalEstiba = dto.IdPersonalEstiba,
            IdCuadrilla = dto.IdCuadrilla,
            Asistencia = dto.Asistencia,
            HoraInicioReal = dto.HoraInicioReal,
            HoraFinReal = dto.HoraFinReal,
            HorasTrabajadas = dto.HorasTrabajadas,
            EstadoAltaBps = dto.EstadoAltaBps,
            Observaciones = dto.Observaciones
        };

        context.DetallesCitacionEstiba.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return (await DetallesConRelaciones()
            .FirstAsync(d => d.IdDetalleCitacion == entity.IdDetalleCitacion, cancellationToken))
            .ToDto();
    }

    public async Task<DetalleCitacionEstibaResponseDto?> UpdateDetalleCitacionAsync(int id, DetalleCitacionEstibaUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await DetallesEditables().FirstOrDefaultAsync(d => d.IdDetalleCitacion == id, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        await EnsureDetalleScopeAsync(entity.IdCitacion, entity.IdPersonalEstiba, dto.IdCuadrilla, dto.IdLiquidacion, cancellationToken);

        entity.IdCuadrilla = dto.IdCuadrilla;
        entity.IdLiquidacion = dto.IdLiquidacion;
        entity.Asistencia = dto.Asistencia;
        entity.HoraInicioReal = dto.HoraInicioReal;
        entity.HoraFinReal = dto.HoraFinReal;
        entity.HorasTrabajadas = dto.HorasTrabajadas;
        entity.EstadoAltaBps = dto.EstadoAltaBps;
        entity.Observaciones = dto.Observaciones;
        await context.SaveChangesAsync(cancellationToken);

        return (await DetallesConRelaciones()
            .FirstAsync(d => d.IdDetalleCitacion == id, cancellationToken))
            .ToDto();
    }

    public async Task<PagedResponseDto<LiquidacionEstibaResponseDto>> GetLiquidacionesAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default) =>
        await LiquidacionesPermitidas()
            .AsNoTracking()
            .Include(l => l.Empresa)
            .OrderByDescending(l => l.PeriodoHasta)
            .Select(l => l.ToDto())
            .ToPagedResponseAsync(pagination, cancellationToken);

    public async Task<LiquidacionEstibaResponseDto> CreateLiquidacionAsync(LiquidacionEstibaCreateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureEmpresaPermitidaAsync(dto.IdEmpresa, cancellationToken);

        var entity = new LiquidacionEstiba
        {
            IdEmpresa = dto.IdEmpresa,
            PeriodoDesde = dto.PeriodoDesde,
            PeriodoHasta = dto.PeriodoHasta,
            Estado = dto.Estado,
            Observaciones = dto.Observaciones
        };

        context.LiquidacionesEstiba.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return (await LiquidacionesConEmpresa().FirstAsync(l => l.IdLiquidacion == entity.IdLiquidacion, cancellationToken)).ToDto();
    }

    public async Task<LiquidacionEstibaResponseDto?> UpdateLiquidacionAsync(int id, LiquidacionEstibaUpdateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureEmpresaPermitidaAsync(dto.IdEmpresa, cancellationToken);

        var entity = await LiquidacionesPermitidas().FirstOrDefaultAsync(l => l.IdLiquidacion == id, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        entity.IdEmpresa = dto.IdEmpresa;
        entity.PeriodoDesde = dto.PeriodoDesde;
        entity.PeriodoHasta = dto.PeriodoHasta;
        entity.TotalHoras = dto.TotalHoras;
        entity.Estado = dto.Estado;
        entity.Observaciones = dto.Observaciones;
        await context.SaveChangesAsync(cancellationToken);

        return (await LiquidacionesConEmpresa().FirstAsync(l => l.IdLiquidacion == id, cancellationToken)).ToDto();
    }

    private IQueryable<PersonalEstiba> PersonalPermitido()
    {
        var query = context.PersonalEstiba.AsQueryable();
        return currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
            ? query
            : query.Where(p => p.IdEmpresa == currentUser.IdEmpresa.Value);
    }

    private IQueryable<PersonalEstiba> PersonalConEmpresa() =>
        context.PersonalEstiba.AsNoTracking().Include(p => p.Empresa);

    private IQueryable<Cuadrilla> CuadrillasPermitidas()
    {
        var query = context.Cuadrillas.AsQueryable();
        return currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
            ? query
            : query.Where(c => c.IdEmpresa == currentUser.IdEmpresa.Value);
    }

    private IQueryable<Cuadrilla> CuadrillasConEmpresa() =>
        context.Cuadrillas.AsNoTracking().Include(c => c.Empresa);

    private IQueryable<CuadrillaPersonal> CuadrillasPersonalConRelaciones() =>
        context.CuadrillasPersonal
            .AsNoTracking()
            .Include(cp => cp.Cuadrilla)
            .Include(cp => cp.PersonalEstiba);

    private IQueryable<CitacionEstiba> CitacionesPermitidas()
    {
        var query = context.CitacionesEstiba.AsQueryable();
        return currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
            ? query
            : query.Where(c => c.IdEmpresa == currentUser.IdEmpresa.Value);
    }

    private IQueryable<CitacionEstiba> CitacionesConRelaciones() =>
        CitacionesPermitidas()
            .AsNoTracking()
            .Include(c => c.Empresa)
            .Include(c => c.Cliente)
            .Include(c => c.EstadoCitacion);

    private IQueryable<DetalleCitacionEstiba> DetallesConRelaciones() =>
        DetallesPermitidos()
            .AsNoTracking()
            .Include(d => d.Citacion)
            .Include(d => d.PersonalEstiba)
            .Include(d => d.Cuadrilla)
            .Include(d => d.Liquidacion);

    private IQueryable<DetalleCitacionEstiba> DetallesEditables() => DetallesPermitidos();

    private IQueryable<DetalleCitacionEstiba> DetallesPermitidos()
    {
        var query = context.DetallesCitacionEstiba.AsQueryable();
        return currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
            ? query
            : query.Where(d => d.Citacion.IdEmpresa == currentUser.IdEmpresa.Value);
    }

    private IQueryable<LiquidacionEstiba> LiquidacionesPermitidas()
    {
        var query = context.LiquidacionesEstiba.AsQueryable();
        return currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
            ? query
            : query.Where(l => l.IdEmpresa == currentUser.IdEmpresa.Value);
    }

    private IQueryable<LiquidacionEstiba> LiquidacionesConEmpresa() =>
        context.LiquidacionesEstiba.AsNoTracking().Include(l => l.Empresa);

    private async Task EnsureCitacionPayloadAsync(int idEmpresa, int? idCliente, CancellationToken cancellationToken)
    {
        await EnsureEmpresaPermitidaAsync(idEmpresa, cancellationToken);

        if (!idCliente.HasValue)
        {
            return;
        }

        var clienteValido = await context.Clientes.AnyAsync(
            c => c.IdCliente == idCliente.Value && c.IdEmpresa == idEmpresa && c.Activo,
            cancellationToken);

        if (!clienteValido)
        {
            throw new ArgumentException("El cliente indicado no pertenece a la empresa de la citación.");
        }
    }

    private async Task EnsureDetalleNoDuplicadoAsync(
        int idCitacion,
        int idPersonalEstiba,
        CancellationToken cancellationToken)
    {
        var yaExiste = await DetallesPermitidos().AnyAsync(
            d => d.IdCitacion == idCitacion && d.IdPersonalEstiba == idPersonalEstiba,
            cancellationToken);

        if (yaExiste)
        {
            throw new ArgumentException("Ese personal ya está agregado al detalle de la citación.");
        }
    }

    private async Task EnsureDetalleScopeAsync(
        int idCitacion,
        int idPersonalEstiba,
        int? idCuadrilla,
        int? idLiquidacion,
        CancellationToken cancellationToken)
    {
        var idEmpresaCitacion = await CitacionesPermitidas()
            .Where(c => c.IdCitacion == idCitacion)
            .Select(c => (int?)c.IdEmpresa)
            .FirstOrDefaultAsync(cancellationToken);

        var idEmpresaPersonal = await PersonalPermitido()
            .Where(p => p.IdPersonalEstiba == idPersonalEstiba)
            .Select(p => (int?)p.IdEmpresa)
            .FirstOrDefaultAsync(cancellationToken);

        if (idEmpresaCitacion is null || idEmpresaPersonal is null)
        {
            throw new ArgumentException("La citación o el personal indicado no existe.");
        }

        if (idEmpresaCitacion != idEmpresaPersonal)
        {
            throw new ArgumentException("La citación y el personal deben pertenecer a la misma empresa.");
        }

        if (idCuadrilla.HasValue)
        {
            var cuadrillaValida = await CuadrillasPermitidas().AnyAsync(
                c => c.IdCuadrilla == idCuadrilla.Value && c.IdEmpresa == idEmpresaCitacion,
                cancellationToken);

            if (!cuadrillaValida)
            {
                throw new ArgumentException("La cuadrilla no pertenece a la empresa de la citación.");
            }
        }

        if (idLiquidacion.HasValue)
        {
            var liquidacionValida = await LiquidacionesPermitidas().AnyAsync(
                l => l.IdLiquidacion == idLiquidacion.Value && l.IdEmpresa == idEmpresaCitacion,
                cancellationToken);

            if (!liquidacionValida)
            {
                throw new ArgumentException("La liquidación no pertenece a la empresa de la citación.");
            }
        }
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
}
