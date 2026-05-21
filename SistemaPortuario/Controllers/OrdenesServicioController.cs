using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPortuario.DTOs;
using SistemaPortuario.Security;
using SistemaPortuario.Services;

namespace SistemaPortuario.Controllers;

[ApiController]
[Route("api/ordenes-servicio")]
[Authorize(Roles = AppRoles.OrdenesLectura)]
/// <summary>
/// Endpoints para ordenes de servicio: consulta, asignacion, cierre y facturacion.
/// </summary>
public class OrdenesServicioController(IOrdenServicioService service) : ControllerBase
{
    [HttpGet("estados")]
    public async Task<ActionResult<List<EstadoOrdenServicioResponseDto>>> GetEstados(CancellationToken cancellationToken) =>
        Ok(await service.GetEstadosAsync(cancellationToken));

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<OrdenServicioResponseDto>>> GetAll([FromQuery] PaginationRequestDto pagination, CancellationToken cancellationToken) =>
        Ok(await service.GetAllAsync(pagination, cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrdenServicioResponseDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = $"{AppRoles.Administrador},{AppRoles.Encargado}")]
    public async Task<ActionResult<OrdenServicioResponseDto>> Create(OrdenServicioCreateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.IdOrdenServicio }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = $"{AppRoles.Administrador},{AppRoles.Encargado}")]
    public async Task<ActionResult<OrdenServicioResponseDto>> Update(int id, OrdenServicioUpdateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, dto, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPatch("{id:int}/finalizar")]
    [Authorize(Roles = $"{AppRoles.Administrador},{AppRoles.Encargado},{AppRoles.Operario}")]
    public async Task<ActionResult<OrdenServicioResponseDto>> Finalizar(int id, FinalizarOrdenServicioDto dto, CancellationToken cancellationToken)
    {
        var result = await service.FinalizarAsync(id, dto, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("facturacion")]
    [Authorize(Roles = AppRoles.Administracion)]
    public async Task<ActionResult<FacturacionResponseDto>> CrearFacturacion(FacturacionCreateDto dto, CancellationToken cancellationToken) =>
        Ok(await service.CrearFacturacionAsync(dto, cancellationToken));
}
