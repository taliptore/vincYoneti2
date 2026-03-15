using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface ICraneRepository
{
    Task<Crane?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Crane>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Crane> AddAsync(Crane entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Crane entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Crane entity, CancellationToken cancellationToken = default);
}
