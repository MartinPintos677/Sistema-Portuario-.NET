using Microsoft.EntityFrameworkCore;
using SistemaPortuario.Data;
using SistemaPortuario.DTOs;
using SistemaPortuario.Models;

namespace SistemaPortuario.Services;

/// <summary>
/// Servicio para clientes.
/// Controla que las operaciónes queden dentro del alcance de empresa permitido.
/// </summary>
public class ClienteService(SistemaPortuarioDbContext context, ICurrentUserService currentUser) : IClienteService
{
    public async Task<PagedResponseDto<ClienteResponseDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default) =>
        await ClientesConRelaciones()
            .OrderBy(c => c.RazonSocial)
            .Select(c => c.ToDto())
            .ToPagedResponseAsync(pagination, cancellationToken);

    public async Task<ClienteResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        (await ClientesConRelaciones().FirstOrDefaultAsync(c => c.IdCliente == id, cancellationToken))?.ToDto();

    public async Task<ClienteResponseDto> CreateAsync(ClienteCreateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureEmpresaPermitidaAsync(dto.IdEmpresa, cancellationToken);

        var entity = new Cliente
        {
            IdEmpresa = dto.IdEmpresa,
            RazonSocial = dto.RazonSocial,
            Rut = dto.Rut,
            NombreContacto = dto.NombreContacto,
            Correo = dto.Correo,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion
        };

        context.Clientes.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return (await GetByIdAsync(entity.IdCliente, cancellationToken))!;
    }

    public async Task<ClienteResponseDto?> UpdateAsync(int id, ClienteUpdateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureEmpresaPermitidaAsync(dto.IdEmpresa, cancellationToken);

        var entity = await ClientesEditables().FirstOrDefaultAsync(c => c.IdCliente == id, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        entity.IdEmpresa = dto.IdEmpresa;
        entity.RazonSocial = dto.RazonSocial;
        entity.Rut = dto.Rut;
        entity.NombreContacto = dto.NombreContacto;
        entity.Correo = dto.Correo;
        entity.Telefono = dto.Telefono;
        entity.Direccion = dto.Direccion;
        entity.Activo = dto.Activo;
        await context.SaveChangesAsync(cancellationToken);
        return await GetByIdAsync(id, cancellationToken);
    }

    private IQueryable<Cliente> ClientesConRelaciones() =>
        ClientesPermitidos()
            .AsNoTracking()
            .Include(c => c.Empresa);

    private IQueryable<Cliente> ClientesPermitidos()
    {
        var query = context.Clientes.AsQueryable();
        return currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
            ? query
            : query.Where(c => c.IdEmpresa == currentUser.IdEmpresa.Value);
    }

    private IQueryable<Cliente> ClientesEditables()
    {
        var query = context.Clientes.AsQueryable();
        return currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
            ? query
            : query.Where(c => c.IdEmpresa == currentUser.IdEmpresa.Value);
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

