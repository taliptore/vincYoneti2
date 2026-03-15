using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface IAnnouncementRepository
{
    Task<Announcement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Announcement>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Announcement> AddAsync(Announcement entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Announcement entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Announcement entity, CancellationToken cancellationToken = default);
}
