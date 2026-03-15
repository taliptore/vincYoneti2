using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Company>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Company> AddAsync(Company entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Company entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Company entity, CancellationToken cancellationToken = default);
}
