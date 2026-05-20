using Microsoft.EntityFrameworkCore;
using SistemaPortuario.Data;
using SistemaPortuario.DTOs;
using SistemaPortuario.Models;

namespace SistemaPortuario.Services;

public class EmpresaService(SistemaPortuarioDbContext context, ICurrentUserService currentUser) : IEmpresaService
{
    public async Task<PagedResponseDto<EmpresaResponseDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default) =>
        await EmpresasPermitidas()
            .AsNoTracking()
            .OrderBy(e => e.Nombre)
            .Select(e => e.ToDto())
            .ToPagedResponseAsync(pagination, cancellationToken);

    public async Task<EmpresaResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        (await EmpresasPermitidas().AsNoTracking().FirstOrDefaultAsync(e => e.IdEmpresa == id, cancellationToken))?.ToDto();

    public async Task<EmpresaResponseDto> CreateAsync(EmpresaCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new Empresa
        {
            Nombre = dto.Nombre,
            RazonSocial = dto.RazonSocial,
            Rut = dto.Rut,
            TipoEmpresa = dto.TipoEmpresa
        };

        context.Empresas.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }

    public async Task<EmpresaResponseDto?> UpdateAsync(int id, EmpresaUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await context.Empresas.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return null;
        }

        entity.Nombre = dto.Nombre;
        entity.RazonSocial = dto.RazonSocial;
        entity.Rut = dto.Rut;
        entity.TipoEmpresa = dto.TipoEmpresa;
        entity.Activa = dto.Activa;
        await context.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }

    private IQueryable<Empresa> EmpresasPermitidas()
    {
        var query = context.Empresas.AsQueryable();
        return currentUser.IsAdministrador || !currentUser.IdEmpresa.HasValue
            ? query
            : query.Where(e => e.IdEmpresa == currentUser.IdEmpresa.Value);
    }
}
