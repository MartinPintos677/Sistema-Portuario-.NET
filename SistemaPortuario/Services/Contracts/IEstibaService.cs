using SistemaPortuario.DTOs;

namespace SistemaPortuario.Services;

public interface IEstibaService
{
    Task<PagedResponseDto<PersonalEstibaResponseDto>> GetPersonalAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default);
    Task<PersonalEstibaResponseDto> CreatePersonalAsync(PersonalEstibaCreateDto dto, CancellationToken cancellationToken = default);
    Task<PersonalEstibaResponseDto?> UpdatePersonalAsync(int id, PersonalEstibaUpdateDto dto, CancellationToken cancellationToken = default);
    Task<PagedResponseDto<CuadrillaResponseDto>> GetCuadrillasAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default);
    Task<CuadrillaResponseDto> CreateCuadrillaAsync(CuadrillaCreateDto dto, CancellationToken cancellationToken = default);
    Task<CuadrillaResponseDto?> UpdateCuadrillaAsync(int id, CuadrillaUpdateDto dto, CancellationToken cancellationToken = default);
    Task<CuadrillaPersonalResponseDto> AsignarPersonalAsync(CuadrillaPersonalCreateDto dto, CancellationToken cancellationToken = default);
    Task<List<EstadoCitacionResponseDto>> GetEstadosCitacionAsync(CancellationToken cancellationToken = default);
    Task<PagedResponseDto<CitacionEstibaResponseDto>> GetCitacionesAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default);
    Task<CitacionEstibaResponseDto> CreateCitacionAsync(CitacionEstibaCreateDto dto, CancellationToken cancellationToken = default);
    Task<CitacionEstibaResponseDto?> UpdateCitacionAsync(int id, CitacionEstibaUpdateDto dto, CancellationToken cancellationToken = default);
    Task<List<DetalleCitacionEstibaResponseDto>> GetDetallesCitacionAsync(int idCitacion, CancellationToken cancellationToken = default);
    Task<DetalleCitacionEstibaResponseDto> CreateDetalleCitacionAsync(DetalleCitacionEstibaCreateDto dto, CancellationToken cancellationToken = default);
    Task<DetalleCitacionEstibaResponseDto?> UpdateDetalleCitacionAsync(int id, DetalleCitacionEstibaUpdateDto dto, CancellationToken cancellationToken = default);
    Task<PagedResponseDto<LiquidacionEstibaResponseDto>> GetLiquidacionesAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default);
    Task<LiquidacionEstibaResponseDto> CreateLiquidacionAsync(LiquidacionEstibaCreateDto dto, CancellationToken cancellationToken = default);
    Task<LiquidacionEstibaResponseDto?> UpdateLiquidacionAsync(int id, LiquidacionEstibaUpdateDto dto, CancellationToken cancellationToken = default);
}
