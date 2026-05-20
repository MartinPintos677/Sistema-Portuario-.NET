using Microsoft.EntityFrameworkCore;
using SistemaPortuario.Data;
using SistemaPortuario.DTOs;

namespace SistemaPortuario.Services;

public class TrazabilidadService(SistemaPortuarioDbContext context) : ITrazabilidadService
{
    private static readonly string[] HiddenEntities = ["RefreshToken"];

    public async Task<PagedResponseDto<TrazabilidadResponseDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default) =>
        await context.Trazabilidades
            .AsNoTracking()
            .Include(t => t.Usuario)
            .Where(t => !HiddenEntities.Contains(t.Entidad))
            .OrderByDescending(t => t.Fecha)
            .Select(t => t.ToDto())
            .ToPagedResponseAsync(pagination, cancellationToken);

    public async Task<PagedResponseDto<TrazabilidadResponseDto>> GetByEntidadAsync(string entidad, string idRegistroAfectado, PaginationRequestDto pagination, CancellationToken cancellationToken = default) =>
        await context.Trazabilidades
            .AsNoTracking()
            .Include(t => t.Usuario)
            .Where(t => t.Entidad == entidad && t.IdRegistroAfectado == idRegistroAfectado)
            .Where(t => !HiddenEntities.Contains(t.Entidad))
            .OrderByDescending(t => t.Fecha)
            .Select(t => t.ToDto())
            .ToPagedResponseAsync(pagination, cancellationToken);
}
