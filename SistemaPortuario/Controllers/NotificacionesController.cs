using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPortuario.DTOs;
using SistemaPortuario.Security;
using SistemaPortuario.Services;

namespace SistemaPortuario.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
/// <summary>
/// Endpoints para notificaciones y actualizacion de estado.
/// </summary>
public class NotificacionesController(INotificacionService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<NotificacionResponseDto>>> GetAll([FromQuery] PaginationRequestDto pagination, CancellationToken cancellationToken) =>
        Ok(await service.GetAllAsync(pagination, cancellationToken));

    [HttpPost]
    [Authorize(Roles = AppRoles.Administracion)]
    public async Task<ActionResult<NotificacionResponseDto>> Create(NotificacionCreateDto dto, CancellationToken cancellationToken) =>
        Ok(await service.CreateAsync(dto, cancellationToken));

    [HttpPatch("{id:int}/estado")]
    public async Task<ActionResult<NotificacionResponseDto>> UpdateEstado(int id, NotificacionUpdateEstadoDto dto, CancellationToken cancellationToken)
    {
        var result = await service.UpdateEstadoAsync(id, dto, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
