using CraneManagementSystem.Application.DTOs.DailyWage;
using CraneManagementSystem.Application.Services;
using CraneManagementSystem.API.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraneManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin, Muhasebe, Operatör, Firma")]
public class DailyWageController : ControllerBase
{
    private readonly DailyWageService _service;

    public DailyWageController(DailyWageService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<DailyWageRecordDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var role = User.GetRole();
        Guid? userId = role == "Operatör" ? User.GetUserId() : null;
        Guid? companyId = role == "Firma" ? User.GetCompanyId() : null;
        var list = await _service.GetAllAsync(userId, companyId, cancellationToken);
        var total = list.Count;
        var items = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return Ok(new PagedResult<DailyWageRecordDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DailyWageRecordDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Muhasebe")]
    [ProducesResponseType(typeof(DailyWageRecordDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] DailyWageRecordCreateDto dto, CancellationToken cancellationToken)
    {
        var item = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin, Muhasebe")]
    [ProducesResponseType(typeof(DailyWageRecordDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] DailyWageRecordUpdateDto dto, CancellationToken cancellationToken)
    {
        var item = await _service.UpdateAsync(id, dto, cancellationToken);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin, Muhasebe")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var ok = await _service.DeleteAsync(id, cancellationToken);
        return ok ? NoContent() : NotFound();
    }
}
