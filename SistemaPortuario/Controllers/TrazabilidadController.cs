using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPortuario.DTOs;
using SistemaPortuario.Security;
using SistemaPortuario.Services;

namespace SistemaPortuario.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Administrador)]
/// <summary>
/// Endpoints de auditoría para revisar cambios registrados por el DbContext.
/// </summary>
public class TrazabilidadController(ITrazabilidadService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<TrazabilidadResponseDto>>> GetAll([FromQuery] PaginationRequestDto pagination, CancellationToken cancellationToken) =>
        Ok(await service.GetAllAsync(pagination, cancellationToken));

    [HttpGet("{entidad}/{idRegistroAfectado}")]
    public async Task<ActionResult<PagedResponseDto<TrazabilidadResponseDto>>> GetByEntidad(string entidad, string idRegistroAfectado, [FromQuery] PaginationRequestDto pagination, CancellationToken cancellationToken) =>
        Ok(await service.GetByEntidadAsync(entidad, idRegistroAfectado, pagination, cancellationToken));
}

