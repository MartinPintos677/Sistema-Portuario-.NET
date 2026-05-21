using SistemaPortuario.DTOs;

namespace SistemaPortuario.Services;

/// <summary>
/// Contrato de consulta de auditoría y trazabilidad.
/// </summary>
public interface ITrazabilidadService
{
    Task<PagedResponseDto<TrazabilidadResponseDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default);
    Task<PagedResponseDto<TrazabilidadResponseDto>> GetByEntidadAsync(string entidad, string idRegistroAfectado, PaginationRequestDto pagination, CancellationToken cancellationToken = default);
}

