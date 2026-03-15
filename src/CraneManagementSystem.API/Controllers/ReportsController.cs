using CraneManagementSystem.Application.DTOs.Report;
using CraneManagementSystem.Application.Services;
using CraneManagementSystem.API.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraneManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin, Muhasebe, Firma")]
public class ReportsController : ControllerBase
{
    private readonly ReportService _service;

    public ReportsController(ReportService service)
    {
        _service = service;
    }

    [HttpGet("income-expense")]
    [ProducesResponseType(typeof(IncomeExpenseReportDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIncomeExpenseReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] Guid? companyId, CancellationToken cancellationToken = default)
    {
        var filterCompanyId = User.GetRole() == "Firma" ? User.GetCompanyId() : companyId;
        var filter = new ReportFilterDto
        {
            StartDate = startDate,
            EndDate = endDate,
            CompanyId = filterCompanyId
        };
        var result = await _service.GetIncomeExpenseReportAsync(filter, cancellationToken);
        return Ok(result);
    }
}
