using Microsoft.EntityFrameworkCore;
using SistemaPortuario.DTOs;

namespace SistemaPortuario.Services;

public static class PaginationExtensions
{
    public static async Task<PagedResponseDto<T>> ToPagedResponseAsync<T>(
        this IQueryable<T> query,
        PaginationRequestDto pagination,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = pagination.GetSafePageNumber();
        var pageSize = pagination.GetSafePageSize();
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var totalPages = totalCount == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResponseDto<T>(items, pageNumber, pageSize, totalCount, totalPages);
    }
}
