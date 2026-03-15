using CraneManagementSystem.Application.DTOs.About;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class AboutService : IAboutService
{
    private const string KeyContent = "AboutUs";
    private const string KeyImageUrl = "AboutUsImageUrl";

    private readonly ISystemSettingRepository _repository;

    public AboutService(ISystemSettingRepository repository)
    {
        _repository = repository;
    }

    public async Task<AboutPageDto> GetAboutAsync(CancellationToken cancellationToken = default)
    {
        var content = (await _repository.GetByKeyAsync(KeyContent, cancellationToken))?.Value;
        var imageUrl = (await _repository.GetByKeyAsync(KeyImageUrl, cancellationToken))?.Value;
        return new AboutPageDto { Content = content, ImageUrl = imageUrl };
    }

    public async Task UpdateAboutAsync(AboutPageDto dto, CancellationToken cancellationToken = default)
    {
        var contentSetting = await _repository.GetByKeyAsync(KeyContent, cancellationToken);
        var imageSetting = await _repository.GetByKeyAsync(KeyImageUrl, cancellationToken);

        if (contentSetting == null)
        {
            contentSetting = new SystemSetting
            {
                Id = Guid.NewGuid(),
                Key = KeyContent,
                Value = dto.Content ?? "",
                CreatedAt = DateTime.UtcNow
            };
            await _repository.AddAsync(contentSetting, cancellationToken);
        }
        else
        {
            contentSetting.Value = dto.Content ?? "";
            contentSetting.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(contentSetting, cancellationToken);
        }

        if (imageSetting == null)
        {
            imageSetting = new SystemSetting
            {
                Id = Guid.NewGuid(),
                Key = KeyImageUrl,
                Value = dto.ImageUrl ?? "",
                CreatedAt = DateTime.UtcNow
            };
            await _repository.AddAsync(imageSetting, cancellationToken);
        }
        else
        {
            imageSetting.Value = dto.ImageUrl ?? "";
            imageSetting.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(imageSetting, cancellationToken);
        }
    }
}
