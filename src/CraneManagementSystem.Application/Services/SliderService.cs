using CraneManagementSystem.Application.DTOs.Slider;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class SliderService
{
    private readonly ISliderRepository _repository;

    public SliderService(ISliderRepository repository)
    {
        _repository = repository;
    }

    public async Task<SliderDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<SliderDto>> GetAllAsync(bool activeOnly, CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetAllAsync(cancellationToken);
        var result = list.Select(MapToDto).ToList();
        if (activeOnly)
            result = result.Where(x => x.IsActive).ToList();
        return result;
    }

    public async Task<SliderDto> CreateAsync(SliderCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new Slider
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            ShortText = dto.ShortText,
            ImageUrl = dto.ImageUrl,
            SortOrder = dto.SortOrder,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };
        await _repository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<SliderDto?> UpdateAsync(Guid id, SliderUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        entity.Title = dto.Title;
        entity.ShortText = dto.ShortText;
        entity.ImageUrl = dto.ImageUrl;
        entity.SortOrder = dto.SortOrder;
        entity.IsActive = dto.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;
        await _repository.DeleteAsync(entity, cancellationToken);
        return true;
    }

    private static SliderDto MapToDto(Slider e) => new()
    {
        Id = e.Id,
        Title = e.Title,
        ShortText = e.ShortText,
        ImageUrl = e.ImageUrl,
        SortOrder = e.SortOrder,
        IsActive = e.IsActive,
        CreatedAt = e.CreatedAt
    };
}
