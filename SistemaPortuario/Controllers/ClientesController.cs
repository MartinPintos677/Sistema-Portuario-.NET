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
/// Endpoints para clientes vinculados a empresas del sistema.
/// </summary>
public class ClientesController(IClienteService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<ClienteResponseDto>>> GetAll([FromQuery] PaginationRequestDto pagination, CancellationToken cancellationToken) =>
        Ok(await service.GetAllAsync(pagination, cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteResponseDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = AppRoles.Administracion)]
    public async Task<ActionResult<ClienteResponseDto>> Create(ClienteCreateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.IdCliente }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = AppRoles.Administracion)]
    public async Task<ActionResult<ClienteResponseDto>> Update(int id, ClienteUpdateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, dto, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
