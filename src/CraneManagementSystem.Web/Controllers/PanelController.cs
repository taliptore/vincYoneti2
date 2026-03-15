using Microsoft.AspNetCore.Mvc;
using CraneManagementSystem.Web.Services;
using CraneManagementSystem.Web.ViewModels;

namespace CraneManagementSystem.Web.Controllers;

public class PanelController : Controller
{
    private readonly IApiClient _api;
    private readonly ILogger<PanelController> _logger;

    public PanelController(IApiClient api, ILogger<PanelController> logger)
    {
        _api = api;
        _logger = logger;
    }

    private IActionResult? RequireAuth()
    {
        var role = ApiClient.GetRoleFromCookie(HttpContext);
        if (string.IsNullOrEmpty(role))
        {
            return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
        }
        ViewData["AuthRole"] = role;
        ViewData["AuthName"] = ApiClient.GetNameFromCookie(HttpContext);
        return null;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var auth = RequireAuth();
        if (auth != null) return auth;

        var summary = await _api.GetAsync<DashboardSummaryViewModel>("api/dashboard/summary", cancellationToken);
        return View(summary ?? new DashboardSummaryViewModel());
    }

    public async Task<IActionResult> Companies(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var auth = RequireAuth();
        if (auth != null) return auth;

        var role = ApiClient.GetRoleFromCookie(HttpContext);
        if (role != "Admin" && role != "Muhasebe" && role != "Firma")
        {
            return Forbid();
        }

        var result = await _api.GetAsync<PagedCompaniesViewModel>($"api/companies?page={page}&pageSize={pageSize}", cancellationToken);
        if (result == null)
            result = new PagedCompaniesViewModel { Page = page, PageSize = pageSize };
        return View(result);
    }
}
