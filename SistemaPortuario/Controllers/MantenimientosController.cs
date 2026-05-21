using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPortuario.DTOs;
using SistemaPortuario.Security;
using SistemaPortuario.Services;

namespace SistemaPortuario.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Taller)]
/// <summary>
/// Endpoints para mantenimiento de maquinaria y sus catalogos.
/// </summary>
public class MantenimientosController(IMantenimientoService service) : ControllerBase
{
    [HttpGet("tipos")]
    public async Task<ActionResult<List<TipoMantenimientoResponseDto>>> GetTipos(CancellationToken cancellationToken) =>
        Ok(await service.GetTiposAsync(cancellationToken));

    [HttpPost("tipos")]
    [Authorize(Roles = AppRoles.Administrador)]
    public async Task<ActionResult<TipoMantenimientoResponseDto>> CreateTipo(TipoMantenimientoCreateDto dto, CancellationToken cancellationToken) =>
        Ok(await service.CreateTipoAsync(dto, cancellationToken));

    [HttpGet("estados")]
    public async Task<ActionResult<List<EstadoMantenimientoResponseDto>>> GetEstados(CancellationToken cancellationToken) =>
        Ok(await service.GetEstadosAsync(cancellationToken));

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<MantenimientoResponseDto>>> GetAll([FromQuery] PaginationRequestDto pagination, CancellationToken cancellationToken) =>
        Ok(await service.GetAllAsync(pagination, cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MantenimientoResponseDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = $"{AppRoles.Administrador},{AppRoles.Encargado}")]
    public async Task<ActionResult<MantenimientoResponseDto>> Create(MantenimientoCreateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.IdMantenimiento }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = $"{AppRoles.Administrador},{AppRoles.Encargado}")]
    public async Task<ActionResult<MantenimientoResponseDto>> Update(int id, MantenimientoUpdateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, dto, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
