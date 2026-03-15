using CraneManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraneManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class HomeController : ControllerBase
{
    private readonly IHomePageService _homePageService;

    public HomeController(IHomePageService homePageService)
    {
        _homePageService = homePageService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(Application.DTOs.Home.HomePageDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHomePage(CancellationToken cancellationToken)
    {
        var result = await _homePageService.GetHomePageAsync(cancellationToken);
        return Ok(result);
    }
}
