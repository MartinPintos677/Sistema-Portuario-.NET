using SistemaPortuario.DTOs;

namespace SistemaPortuario.Services;

public interface ITrazabilidadService
{
    Task<PagedResponseDto<TrazabilidadResponseDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default);
    Task<PagedResponseDto<TrazabilidadResponseDto>> GetByEntidadAsync(string entidad, string idRegistroAfectado, PaginationRequestDto pagination, CancellationToken cancellationToken = default);
}
