using SistemaPortuario.DTOs;

namespace SistemaPortuario.Services;

public interface IEmpresaService
{
    Task<PagedResponseDto<EmpresaResponseDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default);
    Task<EmpresaResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<EmpresaResponseDto> CreateAsync(EmpresaCreateDto dto, CancellationToken cancellationToken = default);
    Task<EmpresaResponseDto?> UpdateAsync(int id, EmpresaUpdateDto dto, CancellationToken cancellationToken = default);
}
