using System.ComponentModel.DataAnnotations;

namespace SistemaPortuario.DTOs;

/// <summary>
/// Parametros de paginacion recibidos por listados.
/// Los metodos seguros evitan paginas invalidas o tamanos excesivos.
/// </summary>
public class PaginationRequestDto
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 20;

    public int GetSafePageNumber() => PageNumber < 1 ? 1 : PageNumber;

    public int GetSafePageSize() => Math.Clamp(PageSize, 1, 100);
}

/// <summary>
/// Formato comun de respuesta paginada consumido por las tablas del frontend.
/// </summary>
public record PagedResponseDto<T>(
    IReadOnlyList<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
