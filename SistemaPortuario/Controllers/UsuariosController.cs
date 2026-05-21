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
/// Endpoints para usuarios, roles y cambio de estado de cuentas.
/// </summary>
public class UsuariosController(IUsuarioService service) : ControllerBase
{
    [HttpGet("roles")]
    [Authorize(Roles = AppRoles.Administrador)]
    public async Task<ActionResult<List<RolResponseDto>>> GetRoles(CancellationToken cancellationToken) =>
        Ok(await service.GetRolesAsync(cancellationToken));

    [HttpGet]
    [Authorize(Roles = AppRoles.GestionOperativa)]
    public async Task<ActionResult<PagedResponseDto<UsuarioResponseDto>>> GetAll([FromQuery] PaginationRequestDto pagination, CancellationToken cancellationToken) =>
        Ok(await service.GetAllAsync(pagination, cancellationToken));

    [HttpGet("{id:int}")]
    [Authorize(Roles = AppRoles.GestionOperativa)]
    public async Task<ActionResult<UsuarioResponseDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = AppRoles.Administrador)]
    public async Task<ActionResult<UsuarioResponseDto>> Create(UsuarioCreateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.IdUsuario }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = AppRoles.Administrador)]
    public async Task<ActionResult<UsuarioResponseDto>> Update(int id, UsuarioUpdateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, dto, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPatch("{id:int}/activo")]
    [Authorize(Roles = AppRoles.Administrador)]
    public async Task<IActionResult> SetActivo(int id, [FromBody] bool activo, CancellationToken cancellationToken)
    {
        var result = await service.SetActivoAsync(id, activo, cancellationToken);
        return result ? NoContent() : NotFound();
    }
}
