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
/// Endpoints para alta, consulta y mantenimiento de empresas.
/// </summary>
public class EmpresasController(IEmpresaService service) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = AppRoles.GestionOperativa)]
    public async Task<ActionResult<PagedResponseDto<EmpresaResponseDto>>> GetAll([FromQuery] PaginationRequestDto pagination, CancellationToken cancellationToken) =>
        Ok(await service.GetAllAsync(pagination, cancellationToken));

    [HttpGet("{id:int}")]
    [Authorize(Roles = AppRoles.GestionOperativa)]
    public async Task<ActionResult<EmpresaResponseDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = AppRoles.Administrador)]
    public async Task<ActionResult<EmpresaResponseDto>> Create(EmpresaCreateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.IdEmpresa }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = AppRoles.Administrador)]
    public async Task<ActionResult<EmpresaResponseDto>> Update(int id, EmpresaUpdateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, dto, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
