using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPortuario.DTOs;
using SistemaPortuario.Security;
using SistemaPortuario.Services;

namespace SistemaPortuario.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MaquinariasController(IMaquinariaService service) : ControllerBase
{
    [HttpGet("tipos")]
    [Authorize(Roles = AppRoles.GestionOperativa)]
    public async Task<ActionResult<List<TipoMaquinariaResponseDto>>> GetTipos(CancellationToken cancellationToken) =>
        Ok(await service.GetTiposAsync(cancellationToken));

    [HttpPost("tipos")]
    [Authorize(Roles = AppRoles.Administrador)]
    public async Task<ActionResult<TipoMaquinariaResponseDto>> CreateTipo(TipoMaquinariaCreateDto dto, CancellationToken cancellationToken) =>
        Ok(await service.CreateTipoAsync(dto, cancellationToken));

    [HttpGet]
    [Authorize(Roles = AppRoles.GestionOperativa)]
    public async Task<ActionResult<PagedResponseDto<MaquinariaResponseDto>>> GetAll([FromQuery] PaginationRequestDto pagination, CancellationToken cancellationToken) =>
        Ok(await service.GetAllAsync(pagination, cancellationToken));

    [HttpGet("{id:int}")]
    [Authorize(Roles = AppRoles.GestionOperativa)]
    public async Task<ActionResult<MaquinariaResponseDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = $"{AppRoles.Administrador},{AppRoles.Encargado}")]
    public async Task<ActionResult<MaquinariaResponseDto>> Create(MaquinariaCreateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.IdMaquinaria }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = $"{AppRoles.Administrador},{AppRoles.Encargado}")]
    public async Task<ActionResult<MaquinariaResponseDto>> Update(int id, MaquinariaUpdateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, dto, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("registros-horas")]
    [Authorize(Roles = $"{AppRoles.Administrador},{AppRoles.Encargado},{AppRoles.Operario}")]
    public async Task<ActionResult<RegistroHorasMaquinariaResponseDto>> RegistrarHoras(RegistroHorasMaquinariaCreateDto dto, CancellationToken cancellationToken) =>
        Ok(await service.RegistrarHorasAsync(dto, cancellationToken));
}
