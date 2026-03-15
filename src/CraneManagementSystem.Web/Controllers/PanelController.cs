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

    private IActionResult? RequireRole(params string[] roles)
    {
        var role = ApiClient.GetRoleFromCookie(HttpContext);
        if (string.IsNullOrEmpty(role) || !roles.Contains(role))
            return Forbid();
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

    public IActionResult Cranes()
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin", "Muhasebe", "Operatör", "Firma") is { } forbid) return forbid;
        return View("ModulePlaceholder", (object)"Vinçler");
    }

    public IActionResult Operators()
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin", "Operatör") is { } forbid) return forbid;
        return View("ModulePlaceholder", (object)"Operatörler");
    }

    public IActionResult ConstructionSites()
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin", "Muhasebe", "Operatör") is { } forbid) return forbid;
        return View("ModulePlaceholder", (object)"Şantiyeler");
    }

    public IActionResult WorkPlans()
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin", "Muhasebe", "Operatör") is { } forbid) return forbid;
        return View("ModulePlaceholder", (object)"İş Planlama");
    }

    public IActionResult ProgressPayments()
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin", "Muhasebe", "Operatör", "Firma") is { } forbid) return forbid;
        return View("ModulePlaceholder", (object)"Hakediş");
    }

    public IActionResult DailyWages()
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin", "Muhasebe", "Operatör") is { } forbid) return forbid;
        return View("ModulePlaceholder", (object)"Yevmiye");
    }

    public IActionResult Overtimes()
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin", "Muhasebe", "Operatör") is { } forbid) return forbid;
        return View("ModulePlaceholder", (object)"Mesai");
    }

    public IActionResult IncomeExpenses()
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin", "Muhasebe", "Operatör") is { } forbid) return forbid;
        return View("ModulePlaceholder", (object)"Gelir-Gider");
    }

    public IActionResult FuelTracking()
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin", "Muhasebe", "Operatör") is { } forbid) return forbid;
        return View("ModulePlaceholder", (object)"Yakıt Takibi");
    }

    public IActionResult Maintenance()
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin", "Operatör") is { } forbid) return forbid;
        return View("ModulePlaceholder", (object)"Bakım");
    }

    public IActionResult Reports()
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin", "Muhasebe", "Operatör", "Firma") is { } forbid) return forbid;
        return View("ModulePlaceholder", (object)"Raporlar");
    }

    public IActionResult Users()
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        return View("ModulePlaceholder", (object)"Kullanıcı Yönetimi");
    }

    public IActionResult Settings()
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        return View("ModulePlaceholder", (object)"Sistem Ayarları");
    }

    // ----- CMS (Admin only) -----
    [HttpGet]
    public async Task<IActionResult> Sliders(CancellationToken ct)
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        var list = await _api.GetAsync<List<SliderItemViewModel>>("api/sliders?activeOnly=false", ct) ?? new List<SliderItemViewModel>();
        return View(list);
    }

    [HttpGet]
    public IActionResult SliderCreate()
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        return View("SliderEdit", new SliderEditViewModel { SortOrder = 0, IsActive = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SliderCreate(SliderEditViewModel model, CancellationToken ct)
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        if (string.IsNullOrWhiteSpace(model.Title)) { ModelState.AddModelError("Title", "Başlık gerekli."); return View("SliderEdit", model); }
        var created = await _api.PostAsync<SliderItemViewModel>("api/sliders", new { model.Title, model.ShortText, model.ImageUrl, model.SortOrder, model.IsActive }, ct);
        if (created == null) { ModelState.AddModelError("", "Kayıt oluşturulamadı."); return View("SliderEdit", model); }
        return RedirectToAction(nameof(Sliders));
    }

    [HttpGet]
    public async Task<IActionResult> SliderEdit(Guid id, CancellationToken ct)
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        var item = await _api.GetAsync<SliderItemViewModel>($"api/sliders/{id}", ct);
        if (item == null) return NotFound();
        return View("SliderEdit", new SliderEditViewModel { Id = item.Id, Title = item.Title, ShortText = item.ShortText, ImageUrl = item.ImageUrl, SortOrder = item.SortOrder, IsActive = item.IsActive });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SliderEdit(SliderEditViewModel model, CancellationToken ct)
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        if (!model.Id.HasValue || string.IsNullOrWhiteSpace(model.Title)) { ModelState.AddModelError("Title", "Başlık gerekli."); return View("SliderEdit", model); }
        var updated = await _api.PutAsync<SliderItemViewModel>($"api/sliders/{model.Id}", new { model.Title, model.ShortText, model.ImageUrl, model.SortOrder, model.IsActive }, ct);
        if (updated == null) { ModelState.AddModelError("", "Güncellenemedi."); return View("SliderEdit", model); }
        return RedirectToAction(nameof(Sliders));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SliderDelete(Guid id, CancellationToken ct)
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        await _api.DeleteAsync($"api/sliders/{id}", ct);
        return RedirectToAction(nameof(Sliders));
    }

    [HttpGet]
    public async Task<IActionResult> News(CancellationToken ct)
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        var list = await _api.GetAsync<PagedResultViewModel<NewsItemViewModel>>("api/news?publishedOnly=false&page=1&pageSize=100", ct);
        var items = list?.Items ?? new List<NewsItemViewModel>();
        return View(items);
    }

    [HttpGet]
    public IActionResult NewsCreate()
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        return View("NewsEdit", new NewsEditViewModel { IsPublished = true });
    }

    [HttpGet]
    public async Task<IActionResult> NewsEdit(Guid id, CancellationToken ct)
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        var item = await _api.GetAsync<NewsItemViewModel>($"api/news/{id}", ct);
        if (item == null) return NotFound();
        return View("NewsEdit", new NewsEditViewModel { Id = item.Id, Title = item.Title, Summary = item.Summary, Body = item.Body, ImageUrl = item.ImageUrl, IsPublished = item.IsPublished, PublishedAt = item.PublishedAt });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NewsCreate(NewsEditViewModel model, CancellationToken ct)
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        if (string.IsNullOrWhiteSpace(model.Title)) { ModelState.AddModelError("Title", "Başlık gerekli."); return View("NewsEdit", model); }
        var created = await _api.PostAsync<NewsItemViewModel>("api/news", new { model.Title, model.Summary, model.Body, model.ImageUrl, model.IsPublished, model.PublishedAt }, ct);
        if (created == null) { ModelState.AddModelError("", "Oluşturulamadı."); return View("NewsEdit", model); }
        return RedirectToAction(nameof(News));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NewsEdit(NewsEditViewModel model, CancellationToken ct)
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        if (!model.Id.HasValue || string.IsNullOrWhiteSpace(model.Title)) { ModelState.AddModelError("Title", "Başlık gerekli."); return View("NewsEdit", model); }
        var updated = await _api.PutAsync<NewsItemViewModel>($"api/news/{model.Id}", new { model.Title, model.Summary, model.Body, model.ImageUrl, model.IsPublished, model.PublishedAt }, ct);
        if (updated == null) { ModelState.AddModelError("", "Güncellenemedi."); return View("NewsEdit", model); }
        return RedirectToAction(nameof(News));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NewsDelete(Guid id, CancellationToken ct)
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        await _api.DeleteAsync($"api/news/{id}", ct);
        return RedirectToAction(nameof(News));
    }

    [HttpGet]
    public async Task<IActionResult> Gallery(CancellationToken ct)
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        var list = await _api.GetAsync<List<GalleryItemViewModel>>("api/gallery?activeOnly=false", ct) ?? new List<GalleryItemViewModel>();
        return View(list);
    }

    [HttpGet]
    public IActionResult GalleryCreate()
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        return View("GalleryEdit", new GalleryItemEditViewModel { SortOrder = 0, IsActive = true });
    }

    [HttpGet]
    public async Task<IActionResult> GalleryEdit(Guid id, CancellationToken ct)
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        var item = await _api.GetAsync<GalleryItemViewModel>($"api/gallery/{id}", ct);
        if (item == null) return NotFound();
        return View("GalleryEdit", new GalleryItemEditViewModel { Id = item.Id, Title = item.Title, ImageUrl = item.ImageUrl, SortOrder = item.SortOrder, IsActive = item.IsActive });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GalleryCreate(GalleryItemEditViewModel model, CancellationToken ct)
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        if (string.IsNullOrWhiteSpace(model.Title)) { ModelState.AddModelError("Title", "Başlık gerekli."); return View("GalleryEdit", model); }
        var created = await _api.PostAsync<GalleryItemViewModel>("api/gallery", new { model.Title, model.ImageUrl, model.SortOrder, model.IsActive }, ct);
        if (created == null) { ModelState.AddModelError("", "Oluşturulamadı."); return View("GalleryEdit", model); }
        return RedirectToAction(nameof(Gallery));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GalleryEdit(GalleryItemEditViewModel model, CancellationToken ct)
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        if (!model.Id.HasValue || string.IsNullOrWhiteSpace(model.Title)) { ModelState.AddModelError("Title", "Başlık gerekli."); return View("GalleryEdit", model); }
        var updated = await _api.PutAsync<GalleryItemViewModel>($"api/gallery/{model.Id}", new { model.Title, model.ImageUrl, model.SortOrder, model.IsActive }, ct);
        if (updated == null) { ModelState.AddModelError("", "Güncellenemedi."); return View("GalleryEdit", model); }
        return RedirectToAction(nameof(Gallery));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GalleryDelete(Guid id, CancellationToken ct)
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        await _api.DeleteAsync($"api/gallery/{id}", ct);
        return RedirectToAction(nameof(Gallery));
    }

    [HttpGet]
    public async Task<IActionResult> About(CancellationToken ct)
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        var dto = await _api.GetAsync<AboutEditViewModel>("api/about", ct) ?? new AboutEditViewModel();
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> About(AboutEditViewModel model, CancellationToken ct)
    {
        var auth = RequireAuth(); if (auth != null) return auth;
        if (RequireRole("Admin") is { } forbid) return forbid;
        var ok = await _api.PutAsync("api/about", new { model.Content, model.ImageUrl }, ct);
        if (!ok) ModelState.AddModelError("", "Kaydedilemedi.");
        return View(model);
    }
}
