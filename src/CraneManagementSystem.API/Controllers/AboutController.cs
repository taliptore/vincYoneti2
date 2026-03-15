using CraneManagementSystem.Application.DTOs.About;
using CraneManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraneManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AboutController : ControllerBase
{
    private readonly IAboutService _service;

    public AboutController(IAboutService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AboutPageDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        var dto = await _service.GetAboutAsync(cancellationToken);
        return Ok(dto);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update([FromBody] AboutPageDto dto, CancellationToken cancellationToken = default)
    {
        await _service.UpdateAboutAsync(dto, cancellationToken);
        return NoContent();
    }
}
