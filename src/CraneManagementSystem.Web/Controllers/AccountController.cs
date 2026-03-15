using Microsoft.AspNetCore.Mvc;
using CraneManagementSystem.Web.Services;
using CraneManagementSystem.Web.ViewModels;

namespace CraneManagementSystem.Web.Controllers;

public class AccountController : Controller
{
    private readonly IApiClient _api;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IApiClient api, ILogger<AccountController> logger)
    {
        _api = api;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ApiClient.GetRoleFromCookie(HttpContext)))
            return RedirectToAction("Index", "Panel", new { area = "" });
        ViewData["ReturnUrl"] = returnUrl ?? Url.Action("Index", "Panel");
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl, CancellationToken cancellationToken)
    {
        ViewData["ReturnUrl"] = returnUrl ?? Url.Action("Index", "Panel");
        if (!ModelState.IsValid)
            return View(model);

        var response = await _api.PostAsync<LoginResponseViewModel>("api/auth/login", new { model.Email, model.Password }, cancellationToken);
        if (response == null || string.IsNullOrEmpty(response.Token))
        {
            ModelState.AddModelError("", "Geçersiz e-posta veya şifre.");
            return View(model);
        }

        ApiClient.SetAuthCookies(HttpContext, response.Token, response.Email, response.FullName, response.Role);
        return LocalRedirect(returnUrl ?? Url.Action("Index", "Panel") ?? "/Panel");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        ApiClient.ClearAuthCookies(HttpContext);
        return RedirectToAction("Index", "Home");
    }
}
