using SistemaPortuario.DTOs;

namespace SistemaPortuario.Services;

public interface IClienteService
{
    Task<PagedResponseDto<ClienteResponseDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default);
    Task<ClienteResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ClienteResponseDto> CreateAsync(ClienteCreateDto dto, CancellationToken cancellationToken = default);
    Task<ClienteResponseDto?> UpdateAsync(int id, ClienteUpdateDto dto, CancellationToken cancellationToken = default);
}
