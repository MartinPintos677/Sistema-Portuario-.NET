using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SistemaPortuario.Data;

namespace SistemaPortuario.Controllers;

[ApiController]
[Route("api/[controller]")]
/// <summary>
/// Endpoints livianos de diagnóstico y precalentamiento para despliegues demo.
/// </summary>
public class HealthController(SistemaPortuarioDbContext context, ILogger<HealthController> logger) : ControllerBase
{
    private static readonly TimeSpan WarmupWindow = TimeSpan.FromSeconds(60);
    private static readonly TimeSpan WarmupDelay = TimeSpan.FromSeconds(5);

    [HttpGet("warmup")]
    [AllowAnonymous]
    public async Task<IActionResult> Warmup(CancellationToken cancellationToken)
    {
        var startedAt = DateTime.UtcNow;
        var attempt = 0;

        while (DateTime.UtcNow - startedAt < WarmupWindow)
        {
            try
            {
                await context.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);
                return NoContent();
            }
            catch (Exception ex) when (IsTransientDatabaseException(ex))
            {
                attempt++;
                logger.LogWarning(
                    ex,
                    "Warmup de base de datos falló temporalmente. Reintentando intento {Attempt}.",
                    attempt);

                await Task.Delay(WarmupDelay, cancellationToken);
            }
        }

        return Problem(
            title: "La base de datos no respondió durante el precalentamiento.",
            statusCode: StatusCodes.Status503ServiceUnavailable);
    }

    private static bool IsTransientDatabaseException(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            if (current is TimeoutException)
            {
                return true;
            }

            if (current is SqlException sqlException &&
                sqlException.Errors.Cast<SqlError>().Any(IsTransientSqlError))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsTransientSqlError(SqlError error) =>
        error.Number is
            -2 or
            20 or
            64 or
            233 or
            4060 or
            4221 or
            40143 or
            40197 or
            40501 or
            40613 or
            49918 or
            49919 or
            49920 or
            10053 or
            10054 or
            10060 or
            11001;
}
