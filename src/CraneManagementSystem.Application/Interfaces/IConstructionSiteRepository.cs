using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface IConstructionSiteRepository
{
    Task<ConstructionSite?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ConstructionSite>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ConstructionSite> AddAsync(ConstructionSite entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(ConstructionSite entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(ConstructionSite entity, CancellationToken cancellationToken = default);
}
