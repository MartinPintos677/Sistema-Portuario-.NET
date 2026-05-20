using SistemaPortuario.DTOs;

namespace SistemaPortuario.Services;

public interface IAdministracionService
{
    Task<List<EstadoTareaResponseDto>> GetEstadosTareaAsync(CancellationToken cancellationToken = default);
    Task<PagedResponseDto<TareaAdministrativaResponseDto>> GetTareasAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default);
    Task<TareaAdministrativaResponseDto?> GetTareaByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TareaAdministrativaResponseDto> CreateTareaAsync(int idCreador, TareaAdministrativaCreateDto dto, CancellationToken cancellationToken = default);
    Task<TareaAdministrativaResponseDto?> UpdateTareaAsync(int id, TareaAdministrativaUpdateDto dto, CancellationToken cancellationToken = default);
    Task<PagedResponseDto<EventoCalendarioResponseDto>> GetEventosAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default);
    Task<EventoCalendarioResponseDto> CreateEventoAsync(int idCreador, EventoCalendarioCreateDto dto, CancellationToken cancellationToken = default);
}
