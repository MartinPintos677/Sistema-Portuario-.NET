using SistemaPortuario.DTOs;

namespace SistemaPortuario.Services;

/// <summary>
/// Contrato de operaciones de ordenes de servicio.
/// </summary>
public interface IOrdenServicioService
{
    Task<List<EstadoOrdenServicioResponseDto>> GetEstadosAsync(CancellationToken cancellationToken = default);
    Task<PagedResponseDto<OrdenServicioResponseDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default);
    Task<OrdenServicioResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<OrdenServicioResponseDto> CreateAsync(OrdenServicioCreateDto dto, CancellationToken cancellationToken = default);
    Task<OrdenServicioResponseDto?> UpdateAsync(int id, OrdenServicioUpdateDto dto, CancellationToken cancellationToken = default);
    Task<OrdenServicioResponseDto?> FinalizarAsync(int id, FinalizarOrdenServicioDto dto, CancellationToken cancellationToken = default);
    Task<FacturacionResponseDto> CrearFacturacionAsync(FacturacionCreateDto dto, CancellationToken cancellationToken = default);
}
