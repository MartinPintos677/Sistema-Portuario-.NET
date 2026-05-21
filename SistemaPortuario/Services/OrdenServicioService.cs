using Microsoft.EntityFrameworkCore;
using SistemaPortuario.Data;
using SistemaPortuario.DTOs;
using SistemaPortuario.Models;
using SistemaPortuario.Security;

namespace SistemaPortuario.Services;

/// <summary>
/// Servicio de órdenes de servicio.
/// Concentra reglas de creación, edición, cierre y facturación de órdenes,
/// respetando alcance por empresa y rol del usuario actual.
/// </summary>
public class OrdenServicioService(SistemaPortuarioDbContext context, ICurrentUserService currentUser) : IOrdenServicioService
{
    public async Task<List<EstadoOrdenServicioResponseDto>> GetEstadosAsync(CancellationToken cancellationToken = default) =>
        await context.EstadosOrdenServicio.AsNoTracking().OrderBy(e => e.IdEstadoOrden).Select(e => e.ToDto()).ToListAsync(cancellationToken);

    public async Task<PagedResponseDto<OrdenServicioResponseDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default) =>
        await OrdenesPermitidas()
            .OrderByDescending(o => o.FechaSolicitud)
            .Select(o => o.ToDto())
            .ToPagedResponseAsync(pagination, cancellationToken);

    public async Task<OrdenServicioResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        (await OrdenesPermitidas().FirstOrDefaultAsync(o => o.IdOrdenServicio == id, cancellationToken))?.ToDto();

    public async Task<OrdenServicioResponseDto> CreateAsync(OrdenServicioCreateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureOrdenPayloadAsync(dto.IdEmpresa, dto.IdCliente, dto.IdEncargado, dto.IdOperario, dto.IdMaquinariaAsignada, dto.IdMaquinariaFacturada, cancellationToken);

        var entity = new OrdenServicio
        {
            IdEmpresa = dto.IdEmpresa,
            IdCliente = dto.IdCliente,
            IdEncargado = dto.IdEncargado,
            IdOperario = dto.IdOperario,
            IdMaquinariaAsignada = dto.IdMaquinariaAsignada,
            IdMaquinariaFacturada = dto.IdMaquinariaFacturada,
            IdEstadoOrden = dto.IdEstadoOrden,
            LugarServicio = dto.LugarServicio,
            TrabajoARealizar = dto.TrabajoARealizar,
            HoraInicioEstimada = dto.HoraInicioEstimada,
            Observaciones = dto.Observaciones,
            RequiereFirmaCliente = dto.RequiereFirmaCliente
        };

        context.OrdenesServicio.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return (await GetByIdAsync(entity.IdOrdenServicio, cancellationToken))!;
    }

    public async Task<OrdenServicioResponseDto?> UpdateAsync(int id, OrdenServicioUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await OrdenesEditables().FirstOrDefaultAsync(o => o.IdOrdenServicio == id, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        await EnsureOrdenPayloadAsync(entity.IdEmpresa, dto.IdCliente, dto.IdEncargado, dto.IdOperario, dto.IdMaquinariaAsignada, dto.IdMaquinariaFacturada, cancellationToken);

        entity.IdCliente = dto.IdCliente;
        entity.IdEncargado = dto.IdEncargado;
        entity.IdOperario = dto.IdOperario;
        entity.IdMaquinariaAsignada = dto.IdMaquinariaAsignada;
        entity.IdMaquinariaFacturada = dto.IdMaquinariaFacturada;
        entity.IdEstadoOrden = dto.IdEstadoOrden;
        entity.LugarServicio = dto.LugarServicio;
        entity.TrabajoARealizar = dto.TrabajoARealizar;
        entity.HoraInicioEstimada = dto.HoraInicioEstimada;
        entity.HoraInicioReal = dto.HoraInicioReal;
        entity.HoraFinalizacion = dto.HoraFinalizacion;
        entity.Observaciones = dto.Observaciones;
        entity.RequiereFirmaCliente = dto.RequiereFirmaCliente;
        entity.EnviadaCliente = dto.EnviadaCliente;
        entity.PrecargadaGSoft = dto.PrecargadaGSoft;

        await context.SaveChangesAsync(cancellationToken);
        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<OrdenServicioResponseDto?> FinalizarAsync(int id, FinalizarOrdenServicioDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await OrdenesEditables().FirstOrDefaultAsync(o => o.IdOrdenServicio == id, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        if (currentUser.Rol == AppRoles.Operario && entity.IdOperario != currentUser.IdUsuario)
        {
            return null;
        }

        entity.HoraFinalizacion = dto.HoraFinalizacion;
        entity.Observaciones = dto.Observaciones;
        entity.IdEstadoOrden = await GetEstadoOrdenIdAsync("PendienteValidacion", cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<FacturacionResponseDto> CrearFacturacionAsync(FacturacionCreateDto dto, CancellationToken cancellationToken = default)
    {
        var orden = await OrdenesEditables().FirstOrDefaultAsync(o => o.IdOrdenServicio == dto.IdOrdenServicio, cancellationToken);

        if (orden is null)
        {
            throw new ArgumentException("La orden indicada no existe.");
        }

        var entity = await context.Facturaciones.FirstOrDefaultAsync(
            f => f.IdOrdenServicio == dto.IdOrdenServicio,
            cancellationToken);

        if (entity is null)
        {
            entity = new Facturacion
            {
                IdOrdenServicio = dto.IdOrdenServicio
            };
            context.Facturaciones.Add(entity);
        }

        entity.FechaEnvio = dto.FechaEnvio;
        entity.Estado = dto.Estado;
        entity.ReferenciaGSoft = dto.ReferenciaGSoft;
        entity.Observaciones = dto.Observaciones;

        orden.IdEstadoOrden = await GetEstadoOrdenIdAsync("Facturada", cancellationToken);
        orden.EnviadaCliente = true;
        orden.PrecargadaGSoft = !string.IsNullOrWhiteSpace(dto.ReferenciaGSoft);
        await context.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }

    private async Task<int> GetEstadoOrdenIdAsync(string nombre, CancellationToken cancellationToken) =>
        await context.EstadosOrdenServicio
            .Where(e => e.Nombre == nombre)
            .Select(e => e.IdEstadoOrden)
            .SingleAsync(cancellationToken);

    private IQueryable<OrdenServicio> OrdenesConRelaciones() =>
        context.OrdenesServicio
            .AsNoTracking()
            .Include(o => o.Empresa)
            .Include(o => o.Cliente)
            .Include(o => o.Encargado)
            .Include(o => o.Operario)
            .Include(o => o.MaquinariaAsignada)
            .Include(o => o.MaquinariaFacturada)
            .Include(o => o.EstadoOrden);

    private IQueryable<OrdenServicio> OrdenesPermitidas()
    {
        // Operario solo ve sus órdenes; otros roles quedan acotados a su empresa.
        var query = OrdenesConRelaciones();
        if (currentUser.Rol == AppRoles.Operario && currentUser.IdUsuario.HasValue)
        {
            return query.Where(o => o.IdOperario == currentUser.IdUsuario.Value);
        }

        return currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
            ? query
            : query.Where(o => o.IdEmpresa == currentUser.IdEmpresa.Value);
    }

    private IQueryable<OrdenServicio> OrdenesEditables()
    {
        // Version trackeada de la consulta porque se usa para modificar entidades.
        var query = context.OrdenesServicio.AsQueryable();
        if (currentUser.Rol == AppRoles.Operario && currentUser.IdUsuario.HasValue)
        {
            return query.Where(o => o.IdOperario == currentUser.IdUsuario.Value);
        }

        return currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
            ? query
            : query.Where(o => o.IdEmpresa == currentUser.IdEmpresa.Value);
    }

    private async Task EnsureOrdenPayloadAsync(
        int idEmpresa,
        int idCliente,
        int idEncargado,
        int idOperario,
        int idMaquinariaAsignada,
        int? idMaquinariaFacturada,
        CancellationToken cancellationToken)
    {
        // Valida que las referencias de la orden pertenezcan a la misma empresa.
        if (!currentUser.CanAccessEmpresa(idEmpresa))
        {
            throw new UnauthorizedAccessException("No tienes permisos para operar con esa empresa.");
        }

        var empresaValida = await context.Empresas.AnyAsync(e => e.IdEmpresa == idEmpresa && e.Activa, cancellationToken);
        var clienteValido = await context.Clientes.AnyAsync(c => c.IdCliente == idCliente && c.IdEmpresa == idEmpresa && c.Activo, cancellationToken);
        var encargadoValido = await context.Usuarios.AnyAsync(u => u.IdUsuario == idEncargado && u.IdEmpresa == idEmpresa && u.Activo, cancellationToken);
        var operarioValido = await context.Usuarios.AnyAsync(u => u.IdUsuario == idOperario && u.IdEmpresa == idEmpresa && u.Activo, cancellationToken);
        var maquinariaValida = await context.Maquinarias.AnyAsync(m => m.IdMaquinaria == idMaquinariaAsignada && m.IdEmpresa == idEmpresa && m.Activa, cancellationToken);
        var maquinariaFacturadaValida = !idMaquinariaFacturada.HasValue ||
            await context.Maquinarias.AnyAsync(m => m.IdMaquinaria == idMaquinariaFacturada.Value && m.IdEmpresa == idEmpresa && m.Activa, cancellationToken);

        if (!empresaValida || !clienteValido || !encargadoValido || !operarioValido || !maquinariaValida || !maquinariaFacturadaValida)
        {
            throw new ArgumentException("La orden contiene referencias que no pertenecen a la empresa indicada.");
        }
    }
}
