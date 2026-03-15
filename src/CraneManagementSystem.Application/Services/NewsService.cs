using CraneManagementSystem.Application.DTOs.News;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class NewsService
{
    private readonly INewsRepository _repository;

    public NewsService(INewsRepository repository)
    {
        _repository = repository;
    }

    public async Task<NewsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<NewsDto>> GetAllAsync(bool? publishedOnly, CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetAllAsync(cancellationToken);
        var result = list.Select(MapToDto).ToList();
        if (publishedOnly == true)
            result = result.Where(x => x.IsPublished).ToList();
        return result;
    }

    public async Task<NewsDto> CreateAsync(NewsCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new News
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Summary = dto.Summary,
            Body = dto.Body,
            ImageUrl = dto.ImageUrl,
            IsPublished = dto.IsPublished,
            PublishedAt = dto.PublishedAt,
            CreatedAt = DateTime.UtcNow
        };
        await _repository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<NewsDto?> UpdateAsync(Guid id, NewsUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        entity.Title = dto.Title;
        entity.Summary = dto.Summary;
        entity.Body = dto.Body;
        entity.ImageUrl = dto.ImageUrl;
        entity.IsPublished = dto.IsPublished;
        entity.PublishedAt = dto.PublishedAt;
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

    private static NewsDto MapToDto(News e) => new()
    {
        Id = e.Id,
        Title = e.Title,
        Summary = e.Summary,
        Body = e.Body,
        ImageUrl = e.ImageUrl,
        IsPublished = e.IsPublished,
        PublishedAt = e.PublishedAt,
        CreatedAt = e.CreatedAt
    };
}
