using SistemaPortuario.DTOs;

namespace SistemaPortuario.Services;

public interface IMaquinariaService
{
    Task<List<TipoMaquinariaResponseDto>> GetTiposAsync(CancellationToken cancellationToken = default);
    Task<TipoMaquinariaResponseDto> CreateTipoAsync(TipoMaquinariaCreateDto dto, CancellationToken cancellationToken = default);
    Task<PagedResponseDto<MaquinariaResponseDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default);
    Task<MaquinariaResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseDto<RegistroHorasMaquinariaResponseDto>> GetHistorialHorasAsync(int idMaquinaria, PaginationRequestDto pagination, CancellationToken cancellationToken = default);
    Task<MaquinariaResponseDto> CreateAsync(MaquinariaCreateDto dto, CancellationToken cancellationToken = default);
    Task<MaquinariaResponseDto?> UpdateAsync(int id, MaquinariaUpdateDto dto, CancellationToken cancellationToken = default);
    Task<RegistroHorasMaquinariaResponseDto> RegistrarHorasAsync(RegistroHorasMaquinariaCreateDto dto, CancellationToken cancellationToken = default);
}
