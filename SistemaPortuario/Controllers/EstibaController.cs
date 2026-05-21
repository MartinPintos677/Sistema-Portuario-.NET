using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPortuario.DTOs;
using SistemaPortuario.Security;
using SistemaPortuario.Services;

namespace SistemaPortuario.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Estiba)]
/// <summary>
/// Endpoints para operativa de estiba: personal, cuadrillas, citaciones y liquidaciones.
/// </summary>
public class EstibaController(IEstibaService service) : ControllerBase
{
    [HttpGet("personal")]
    public async Task<ActionResult<PagedResponseDto<PersonalEstibaResponseDto>>> GetPersonal([FromQuery] PaginationRequestDto pagination, CancellationToken cancellationToken) =>
        Ok(await service.GetPersonalAsync(pagination, cancellationToken));

    [HttpPost("personal")]
    [Authorize(Roles = AppRoles.Administracion)]
    public async Task<ActionResult<PersonalEstibaResponseDto>> CreatePersonal(PersonalEstibaCreateDto dto, CancellationToken cancellationToken) =>
        Ok(await service.CreatePersonalAsync(dto, cancellationToken));

    [HttpPut("personal/{id:int}")]
    [Authorize(Roles = AppRoles.Administracion)]
    public async Task<ActionResult<PersonalEstibaResponseDto>> UpdatePersonal(int id, PersonalEstibaUpdateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.UpdatePersonalAsync(id, dto, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("cuadrillas")]
    public async Task<ActionResult<PagedResponseDto<CuadrillaResponseDto>>> GetCuadrillas([FromQuery] PaginationRequestDto pagination, CancellationToken cancellationToken) =>
        Ok(await service.GetCuadrillasAsync(pagination, cancellationToken));

    [HttpPost("cuadrillas")]
    [Authorize(Roles = AppRoles.Administracion)]
    public async Task<ActionResult<CuadrillaResponseDto>> CreateCuadrilla(CuadrillaCreateDto dto, CancellationToken cancellationToken) =>
        Ok(await service.CreateCuadrillaAsync(dto, cancellationToken));

    [HttpPut("cuadrillas/{id:int}")]
    [Authorize(Roles = AppRoles.Administracion)]
    public async Task<ActionResult<CuadrillaResponseDto>> UpdateCuadrilla(int id, CuadrillaUpdateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.UpdateCuadrillaAsync(id, dto, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("cuadrillas/personal")]
    [Authorize(Roles = AppRoles.Administracion)]
    public async Task<ActionResult<CuadrillaPersonalResponseDto>> AsignarPersonal(CuadrillaPersonalCreateDto dto, CancellationToken cancellationToken) =>
        Ok(await service.AsignarPersonalAsync(dto, cancellationToken));

    [HttpGet("estados-citacion")]
    public async Task<ActionResult<List<EstadoCitacionResponseDto>>> GetEstadosCitacion(CancellationToken cancellationToken) =>
        Ok(await service.GetEstadosCitacionAsync(cancellationToken));

    [HttpGet("citaciones")]
    public async Task<ActionResult<PagedResponseDto<CitacionEstibaResponseDto>>> GetCitaciones([FromQuery] PaginationRequestDto pagination, CancellationToken cancellationToken) =>
        Ok(await service.GetCitacionesAsync(pagination, cancellationToken));

    [HttpPost("citaciones")]
    public async Task<ActionResult<CitacionEstibaResponseDto>> CreateCitacion(CitacionEstibaCreateDto dto, CancellationToken cancellationToken) =>
        Ok(await service.CreateCitacionAsync(dto, cancellationToken));

    [HttpPut("citaciones/{id:int}")]
    public async Task<ActionResult<CitacionEstibaResponseDto>> UpdateCitacion(int id, CitacionEstibaUpdateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.UpdateCitacionAsync(id, dto, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("citaciones/{id:int}/detalles")]
    public async Task<ActionResult<List<DetalleCitacionEstibaResponseDto>>> GetDetallesCitacion(int id, CancellationToken cancellationToken) =>
        Ok(await service.GetDetallesCitacionAsync(id, cancellationToken));

    [HttpPost("citaciones/detalles")]
    public async Task<ActionResult<DetalleCitacionEstibaResponseDto>> CreateDetalleCitacion(DetalleCitacionEstibaCreateDto dto, CancellationToken cancellationToken) =>
        Ok(await service.CreateDetalleCitacionAsync(dto, cancellationToken));

    [HttpPut("citaciones/detalles/{id:int}")]
    public async Task<ActionResult<DetalleCitacionEstibaResponseDto>> UpdateDetalleCitacion(int id, DetalleCitacionEstibaUpdateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.UpdateDetalleCitacionAsync(id, dto, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("liquidaciones")]
    [Authorize(Roles = AppRoles.Administracion)]
    public async Task<ActionResult<PagedResponseDto<LiquidacionEstibaResponseDto>>> GetLiquidaciones([FromQuery] PaginationRequestDto pagination, CancellationToken cancellationToken) =>
        Ok(await service.GetLiquidacionesAsync(pagination, cancellationToken));

    [HttpPost("liquidaciones")]
    [Authorize(Roles = AppRoles.Administracion)]
    public async Task<ActionResult<LiquidacionEstibaResponseDto>> CreateLiquidacion(LiquidacionEstibaCreateDto dto, CancellationToken cancellationToken) =>
        Ok(await service.CreateLiquidacionAsync(dto, cancellationToken));

    [HttpPut("liquidaciones/{id:int}")]
    [Authorize(Roles = AppRoles.Administracion)]
    public async Task<ActionResult<LiquidacionEstibaResponseDto>> UpdateLiquidacion(int id, LiquidacionEstibaUpdateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.UpdateLiquidacionAsync(id, dto, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
