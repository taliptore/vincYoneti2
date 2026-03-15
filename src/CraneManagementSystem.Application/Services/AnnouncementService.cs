using CraneManagementSystem.Application.DTOs.Announcement;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class AnnouncementService
{
    private readonly IAnnouncementRepository _repository;

    public AnnouncementService(IAnnouncementRepository repository)
    {
        _repository = repository;
    }

    public async Task<AnnouncementDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<AnnouncementDto>> GetAllAsync(bool? pinnedOnly, CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetAllAsync(cancellationToken);
        var result = list.Select(MapToDto).ToList();
        if (pinnedOnly == true)
            result = result.Where(x => x.IsPinned).ToList();
        return result;
    }

    public async Task<AnnouncementDto> CreateAsync(AnnouncementCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new Announcement
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Content = dto.Content,
            IsPinned = dto.IsPinned,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            CreatedAt = DateTime.UtcNow
        };
        await _repository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<AnnouncementDto?> UpdateAsync(Guid id, AnnouncementUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        entity.Title = dto.Title;
        entity.Content = dto.Content;
        entity.IsPinned = dto.IsPinned;
        entity.StartDate = dto.StartDate;
        entity.EndDate = dto.EndDate;
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

    private static AnnouncementDto MapToDto(Announcement e) => new()
    {
        Id = e.Id,
        Title = e.Title,
        Content = e.Content,
        IsPinned = e.IsPinned,
        StartDate = e.StartDate,
        EndDate = e.EndDate,
        CreatedAt = e.CreatedAt
    };
}
