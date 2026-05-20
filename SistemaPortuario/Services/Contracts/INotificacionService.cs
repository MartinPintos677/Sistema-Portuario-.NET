using SistemaPortuario.DTOs;

namespace SistemaPortuario.Services;

public interface INotificacionService
{
    Task<PagedResponseDto<NotificacionResponseDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default);
    Task<NotificacionResponseDto> CreateAsync(NotificacionCreateDto dto, CancellationToken cancellationToken = default);
    Task<NotificacionResponseDto?> UpdateEstadoAsync(int id, NotificacionUpdateEstadoDto dto, CancellationToken cancellationToken = default);
}
