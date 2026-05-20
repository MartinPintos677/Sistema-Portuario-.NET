using Microsoft.EntityFrameworkCore;
using SistemaPortuario.Data;
using SistemaPortuario.DTOs;
using SistemaPortuario.Models;
using SistemaPortuario.Security;

namespace SistemaPortuario.Services;

public class MaquinariaService(SistemaPortuarioDbContext context, ICurrentUserService currentUser) : IMaquinariaService
{
    private const decimal MaintenanceHourStep = 200;

    public async Task<List<TipoMaquinariaResponseDto>> GetTiposAsync(CancellationToken cancellationToken = default) =>
        await context.TiposMaquinaria.AsNoTracking().OrderBy(t => t.Nombre).Select(t => t.ToDto()).ToListAsync(cancellationToken);

    public async Task<TipoMaquinariaResponseDto> CreateTipoAsync(TipoMaquinariaCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new TipoMaquinaria { Nombre = dto.Nombre };
        context.TiposMaquinaria.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }

    public async Task<PagedResponseDto<MaquinariaResponseDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default) =>
        await MaquinariasConRelaciones()
            .OrderBy(m => m.Codigo)
            .Select(m => m.ToDto())
            .ToPagedResponseAsync(pagination, cancellationToken);

    public async Task<MaquinariaResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        (await MaquinariasConRelaciones().FirstOrDefaultAsync(m => m.IdMaquinaria == id, cancellationToken))?.ToDto();

    public async Task<MaquinariaResponseDto> CreateAsync(MaquinariaCreateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureEmpresaPermitidaAsync(dto.IdEmpresa, cancellationToken);

        var entity = new Maquinaria
        {
            IdEmpresa = dto.IdEmpresa,
            IdTipoMaquinaria = dto.IdTipoMaquinaria,
            Codigo = dto.Codigo,
            Nombre = dto.Nombre,
            Marca = dto.Marca,
            Modelo = dto.Modelo,
            Matricula = dto.Matricula
        };

        context.Maquinarias.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return (await GetByIdAsync(entity.IdMaquinaria, cancellationToken))!;
    }

    public async Task<MaquinariaResponseDto?> UpdateAsync(int id, MaquinariaUpdateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureEmpresaPermitidaAsync(dto.IdEmpresa, cancellationToken);

        var entity = await MaquinariasEditables().FirstOrDefaultAsync(m => m.IdMaquinaria == id, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        entity.IdEmpresa = dto.IdEmpresa;
        entity.IdTipoMaquinaria = dto.IdTipoMaquinaria;
        entity.Codigo = dto.Codigo;
        entity.Nombre = dto.Nombre;
        entity.Marca = dto.Marca;
        entity.Modelo = dto.Modelo;
        entity.Matricula = dto.Matricula;
        entity.HorasAcumuladas = dto.HorasAcumuladas;
        entity.Activa = dto.Activa;
        await context.SaveChangesAsync(cancellationToken);
        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<RegistroHorasMaquinariaResponseDto> RegistrarHorasAsync(RegistroHorasMaquinariaCreateDto dto, CancellationToken cancellationToken = default)
    {
        var maquinaria = await MaquinariasEditables().FirstOrDefaultAsync(m => m.IdMaquinaria == dto.IdMaquinaria, cancellationToken);

        if (maquinaria is null)
        {
            throw new ArgumentException("La maquinaria indicada no existe.");
        }

        var horasPrevias = maquinaria.HorasAcumuladas;

        if (dto.IdOrdenServicio.HasValue)
        {
            var orden = await context.OrdenesServicio
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.IdOrdenServicio == dto.IdOrdenServicio.Value, cancellationToken);

            if (orden is null)
            {
                throw new ArgumentException("La orden asociada no existe.");
            }

            if (orden.IdEmpresa != maquinaria.IdEmpresa)
            {
                throw new ArgumentException("La orden y la maquinaria deben pertenecer a la misma empresa.");
            }

            var maquinariaPerteneceAOrden =
                orden.IdMaquinariaAsignada == dto.IdMaquinaria ||
                orden.IdMaquinariaFacturada == dto.IdMaquinaria;

            if (!maquinariaPerteneceAOrden)
            {
                throw new ArgumentException("La maquinaria indicada no esta asignada ni facturada en la orden.");
            }

            if (currentUser.Rol == AppRoles.Operario)
            {
                var puedeRegistrar =
                    currentUser.IdUsuario.HasValue &&
                    orden.IdOperario == currentUser.IdUsuario.Value;

                if (!puedeRegistrar)
                {
                    throw new UnauthorizedAccessException("No tienes permisos para registrar horas en esta orden.");
                }
            }
        }
        else if (currentUser.Rol == AppRoles.Operario)
        {
            throw new UnauthorizedAccessException("El operario debe asociar las horas a una orden propia.");
        }

        var entity = new RegistroHorasMaquinaria
        {
            IdMaquinaria = dto.IdMaquinaria,
            IdOrdenServicio = dto.IdOrdenServicio,
            Fecha = dto.Fecha,
            HorasTrabajadas = dto.HorasTrabajadas,
            Observacion = dto.Observacion
        };

        context.RegistrosHorasMaquinaria.Add(entity);
        maquinaria.HorasAcumuladas += dto.HorasTrabajadas;
        await CrearAlertaMantenimientoSiCorrespondeAsync(maquinaria, horasPrevias, entity, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return (await context.RegistrosHorasMaquinaria
            .AsNoTracking()
            .Include(r => r.Maquinaria)
            .FirstAsync(r => r.IdRegistroHoras == entity.IdRegistroHoras, cancellationToken))
            .ToDto();
    }

    private IQueryable<Maquinaria> MaquinariasConRelaciones() =>
        MaquinariasPermitidas()
            .AsNoTracking()
            .Include(m => m.Empresa)
            .Include(m => m.TipoMaquinaria);

    private IQueryable<Maquinaria> MaquinariasPermitidas()
    {
        var query = context.Maquinarias.AsQueryable();
        return currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
            ? query
            : query.Where(m => m.IdEmpresa == currentUser.IdEmpresa.Value);
    }

    private IQueryable<Maquinaria> MaquinariasEditables()
    {
        var query = context.Maquinarias.AsQueryable();
        return currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
            ? query
            : query.Where(m => m.IdEmpresa == currentUser.IdEmpresa.Value);
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

    private async Task CrearAlertaMantenimientoSiCorrespondeAsync(
        Maquinaria maquinaria,
        decimal horasPrevias,
        RegistroHorasMaquinaria registro,
        CancellationToken cancellationToken)
    {
        var umbralPrevio = Math.Floor(horasPrevias / MaintenanceHourStep);
        var umbralActual = Math.Floor(maquinaria.HorasAcumuladas / MaintenanceHourStep);
        if (umbralActual <= umbralPrevio)
        {
            return;
        }

        var tipoId = await GetOrCreateTipoMantenimiento200HorasAsync(cancellationToken);
        var estadoPendienteId = await context.EstadosMantenimiento
            .Where(e => e.Nombre == "Pendiente")
            .Select(e => e.IdEstadoMantenimiento)
            .SingleAsync(cancellationToken);

        context.Mantenimientos.Add(new Mantenimiento
        {
            IdMaquinaria = maquinaria.IdMaquinaria,
            IdTipoMantenimiento = tipoId,
            IdEstadoMantenimiento = estadoPendienteId,
            RegistroHorasOrigen = registro,
            FechaProgramada = DateOnly.FromDateTime(DateTime.UtcNow.Date),
            Descripcion = $"Revision preventiva por cruce de {umbralActual * MaintenanceHourStep:0} horas acumuladas.",
            HorasMaquinaAlMomento = maquinaria.HorasAcumuladas,
            Observaciones = "Alerta automatica generada al registrar horas de maquinaria."
        });

        if (currentUser.IdUsuario.HasValue)
        {
            context.Notificaciones.Add(new Notificacion
            {
                IdUsuario = currentUser.IdUsuario.Value,
                IdOrdenServicio = registro.IdOrdenServicio,
                Tipo = "Sistema",
                Destinatario = "Sistema",
                Mensaje = $"La maquinaria {maquinaria.Codigo} acumulo {maquinaria.HorasAcumuladas:0.##} horas. Corresponde revisar mantenimiento preventivo.",
                Estado = "Pendiente"
            });
        }
    }

    private async Task<int> GetOrCreateTipoMantenimiento200HorasAsync(CancellationToken cancellationToken)
    {
        const string nombre = "Preventivo 200 horas";
        var existingId = await context.TiposMantenimiento
            .Where(t => t.Nombre == nombre)
            .Select(t => (int?)t.IdTipoMantenimiento)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingId.HasValue)
        {
            return existingId.Value;
        }

        var tipo = new TipoMantenimiento
        {
            Nombre = nombre,
            Descripcion = "Revision preventiva automatica cada 200 horas trabajadas.",
            UmbralHoras = MaintenanceHourStep
        };

        context.TiposMantenimiento.Add(tipo);
        await context.SaveChangesAsync(cancellationToken);
        return tipo.IdTipoMantenimiento;
    }
}
