using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPortuario.DTOs;
using SistemaPortuario.Security;
using SistemaPortuario.Services;

namespace SistemaPortuario.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.GestionOperativa)]
/// <summary>
/// Endpoints para tareas administrativas y eventos de calendario.
/// </summary>
public class AdministracionController(IAdministracionService service) : ControllerBase
{
    [HttpGet("estados-tarea")]
    public async Task<ActionResult<List<EstadoTareaResponseDto>>> GetEstadosTarea(CancellationToken cancellationToken) =>
        Ok(await service.GetEstadosTareaAsync(cancellationToken));

    [HttpGet("tareas")]
    public async Task<ActionResult<PagedResponseDto<TareaAdministrativaResponseDto>>> GetTareas([FromQuery] PaginationRequestDto pagination, CancellationToken cancellationToken) =>
        Ok(await service.GetTareasAsync(pagination, cancellationToken));

    [HttpGet("tareas/{id:int}")]
    public async Task<ActionResult<TareaAdministrativaResponseDto>> GetTareaById(int id, CancellationToken cancellationToken)
    {
        var result = await service.GetTareaByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("tareas")]
    public async Task<ActionResult<TareaAdministrativaResponseDto>> CreateTarea(TareaAdministrativaCreateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.CreateTareaAsync(User.GetUsuarioId(), dto, cancellationToken);
        return CreatedAtAction(nameof(GetTareaById), new { id = result.IdTarea }, result);
    }

    [HttpPut("tareas/{id:int}")]
    public async Task<ActionResult<TareaAdministrativaResponseDto>> UpdateTarea(int id, TareaAdministrativaUpdateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.UpdateTareaAsync(id, dto, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("eventos")]
    public async Task<ActionResult<PagedResponseDto<EventoCalendarioResponseDto>>> GetEventos([FromQuery] PaginationRequestDto pagination, CancellationToken cancellationToken) =>
        Ok(await service.GetEventosAsync(pagination, cancellationToken));

    [HttpPost("eventos")]
    public async Task<ActionResult<EventoCalendarioResponseDto>> CreateEvento(EventoCalendarioCreateDto dto, CancellationToken cancellationToken) =>
        Ok(await service.CreateEventoAsync(User.GetUsuarioId(), dto, cancellationToken));
}

