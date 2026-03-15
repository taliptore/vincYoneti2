using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface IDailyWageRecordRepository
{
    Task<DailyWageRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DailyWageRecord>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DailyWageRecord> AddAsync(DailyWageRecord entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(DailyWageRecord entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(DailyWageRecord entity, CancellationToken cancellationToken = default);
}
