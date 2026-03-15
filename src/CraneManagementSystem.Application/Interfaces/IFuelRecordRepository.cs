using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface IFuelRecordRepository
{
    Task<FuelRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FuelRecord>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<FuelRecord> AddAsync(FuelRecord entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(FuelRecord entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(FuelRecord entity, CancellationToken cancellationToken = default);
}
