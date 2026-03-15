using CraneManagementSystem.Application.DTOs.Gallery;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class GalleryItemService
{
    private readonly IGalleryItemRepository _repository;

    public GalleryItemService(IGalleryItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<GalleryItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<GalleryItemDto>> GetAllAsync(bool activeOnly, CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetAllAsync(cancellationToken);
        var result = list.Select(MapToDto).ToList();
        if (activeOnly)
            result = result.Where(x => x.IsActive).ToList();
        return result;
    }

    public async Task<GalleryItemDto> CreateAsync(GalleryItemCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new GalleryItem
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            ImageUrl = dto.ImageUrl,
            SortOrder = dto.SortOrder,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };
        await _repository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<GalleryItemDto?> UpdateAsync(Guid id, GalleryItemUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        entity.Title = dto.Title;
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

    private static GalleryItemDto MapToDto(GalleryItem e) => new()
    {
        Id = e.Id,
        Title = e.Title,
        ImageUrl = e.ImageUrl,
        SortOrder = e.SortOrder,
        IsActive = e.IsActive,
        CreatedAt = e.CreatedAt
    };
}
