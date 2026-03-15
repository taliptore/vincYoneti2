using Microsoft.AspNetCore.Mvc;
using CraneManagementSystem.Web.Models;
using CraneManagementSystem.Web.Services;
using CraneManagementSystem.Web.ViewModels;

namespace CraneManagementSystem.Web.Controllers;

public class HomeController : Controller
{
    private readonly IApiClient _api;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IApiClient api, ILogger<HomeController> logger)
    {
        _api = api;
        _logger = logger;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await _api.GetAsync<HomePageViewModel>("api/home", cancellationToken)
            ?? new HomePageViewModel { HeroTitle = "TORE VINC", HeroSubtitle = "Vinç Yönetim Sistemi" };
        return View(model);
    }

    [HttpGet]
    public IActionResult Contact()
    {
        return View(new ContactFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact(ContactFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);
        var success = await _api.PostAsync("api/contact", new
        {
            model.Name,
            model.Email,
            model.Phone,
            model.Subject,
            model.Message
        }, cancellationToken);
        if (success)
        {
            TempData["ContactSuccess"] = true;
            return RedirectToAction(nameof(Index));
        }
        ModelState.AddModelError("", "Mesaj gönderilemedi. Lütfen tekrar deneyin.");
        return View(model);
    }

    [HttpGet]
    public IActionResult Appointment()
    {
        return View(new AppointmentFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Appointment(AppointmentFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);
        var success = await _api.PostAsync("api/appointments", new
        {
            model.CustomerName,
            model.Email,
            model.Phone,
            model.PreferredDate,
            model.Notes,
            Status = "Pending"
        }, cancellationToken);
        if (success)
        {
            TempData["AppointmentSuccess"] = true;
            return RedirectToAction(nameof(Index));
        }
        ModelState.AddModelError("", "Randevu talebi gönderilemedi. Lütfen tekrar deneyin.");
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
