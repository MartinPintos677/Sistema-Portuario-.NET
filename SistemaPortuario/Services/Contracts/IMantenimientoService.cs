using SistemaPortuario.DTOs;

namespace SistemaPortuario.Services;

public interface IMantenimientoService
{
    Task<List<TipoMantenimientoResponseDto>> GetTiposAsync(CancellationToken cancellationToken = default);
    Task<TipoMantenimientoResponseDto> CreateTipoAsync(TipoMantenimientoCreateDto dto, CancellationToken cancellationToken = default);
    Task<List<EstadoMantenimientoResponseDto>> GetEstadosAsync(CancellationToken cancellationToken = default);
    Task<PagedResponseDto<MantenimientoResponseDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default);
    Task<MantenimientoResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MantenimientoResponseDto> CreateAsync(MantenimientoCreateDto dto, CancellationToken cancellationToken = default);
    Task<MantenimientoResponseDto?> UpdateAsync(int id, MantenimientoUpdateDto dto, CancellationToken cancellationToken = default);
}
