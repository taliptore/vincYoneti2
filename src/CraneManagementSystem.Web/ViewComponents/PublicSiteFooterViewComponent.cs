using CraneManagementSystem.Web.Services;
using CraneManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CraneManagementSystem.Web.ViewComponents;

public class PublicSiteFooterViewComponent : ViewComponent
{
    private readonly IApiClient _api;

    public PublicSiteFooterViewComponent(IApiClient api)
    {
        _api = api;
    }

    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        var model = HttpContext.Items["PublicSiteSettings"] as HomePageViewModel;
        if (model == null)
            model = await _api.GetAsync<HomePageViewModel>("api/home", cancellationToken);
        return View(model ?? new HomePageViewModel());
    }
}
