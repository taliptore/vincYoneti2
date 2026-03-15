using CraneManagementSystem.Application.DTOs.Home;

namespace CraneManagementSystem.Application.Interfaces;

public interface IHomePageService
{
    Task<HomePageDto> GetHomePageAsync(CancellationToken cancellationToken = default);
}
