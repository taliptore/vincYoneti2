using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface IGalleryItemRepository
{
    Task<GalleryItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<GalleryItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<GalleryItem> AddAsync(GalleryItem entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(GalleryItem entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(GalleryItem entity, CancellationToken cancellationToken = default);
}
