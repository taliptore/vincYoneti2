using CraneManagementSystem.Application.DTOs.Menu;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.API.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraneManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    /// <summary>Kullanıcının rolüne göre menüleri getirir (Vue sidebar için).</summary>
    [HttpGet("user")]
    [ProducesResponseType(typeof(List<MenuDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetForCurrentUser(CancellationToken cancellationToken = default)
    {
        var role = User.GetRole();
        if (string.IsNullOrEmpty(role))
            return Ok(new List<MenuDto>());
        var menus = await _menuService.GetMenusForUserByRoleAsync(role, cancellationToken);
        return Ok(menus);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(List<MenuDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var menus = await _menuService.GetAllAsTreeAsync(cancellationToken);
        return Ok(menus);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(MenuDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var menu = await _menuService.GetByIdAsync(id, cancellationToken);
        return menu == null ? NotFound() : Ok(menu);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(MenuDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] MenuCreateDto dto, CancellationToken cancellationToken = default)
    {
        var menu = await _menuService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = menu.Id }, menu);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(MenuDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] MenuUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var menu = await _menuService.UpdateAsync(id, dto, cancellationToken);
        return menu == null ? NotFound() : Ok(menu);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var ok = await _menuService.DeleteAsync(id, cancellationToken);
        return ok ? NoContent() : NotFound();
    }

    [HttpPut("{id:guid}/reorder")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Reorder(Guid id, [FromQuery] int orderNo, CancellationToken cancellationToken = default)
    {
        await _menuService.ReorderAsync(id, orderNo, cancellationToken);
        return NoContent();
    }
}
