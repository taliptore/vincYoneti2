using CraneManagementSystem.Web.Services;
using CraneManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CraneManagementSystem.Web.ViewComponents;

public class PublicSiteHeaderViewComponent : ViewComponent
{
    private readonly IApiClient _api;

    public PublicSiteHeaderViewComponent(IApiClient api)
    {
        _api = api;
    }

    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        var model = await _api.GetAsync<HomePageViewModel>("api/home", cancellationToken);
        if (model != null)
            HttpContext.Items["PublicSiteSettings"] = model;
        return View(model ?? new HomePageViewModel());
    }
}
