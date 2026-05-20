using SistemaPortuario.DTOs;

namespace SistemaPortuario.Services;

public interface IUsuarioService
{
    Task<List<RolResponseDto>> GetRolesAsync(CancellationToken cancellationToken = default);
    Task<PagedResponseDto<UsuarioResponseDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken cancellationToken = default);
    Task<UsuarioResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UsuarioResponseDto> CreateAsync(UsuarioCreateDto dto, CancellationToken cancellationToken = default);
    Task<UsuarioResponseDto?> UpdateAsync(int id, UsuarioUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> SetActivoAsync(int id, bool activo, CancellationToken cancellationToken = default);
}
