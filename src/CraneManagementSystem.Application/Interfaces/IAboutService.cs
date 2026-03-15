using CraneManagementSystem.Application.DTOs.About;

namespace CraneManagementSystem.Application.Interfaces;

public interface IAboutService
{
    Task<AboutPageDto> GetAboutAsync(CancellationToken cancellationToken = default);
    Task UpdateAboutAsync(AboutPageDto dto, CancellationToken cancellationToken = default);
}
