using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface INewsRepository
{
    Task<News?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<News>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<News> AddAsync(News entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(News entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(News entity, CancellationToken cancellationToken = default);
}
