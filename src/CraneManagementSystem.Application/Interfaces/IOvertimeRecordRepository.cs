using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface IOvertimeRecordRepository
{
    Task<OvertimeRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OvertimeRecord>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<OvertimeRecord> AddAsync(OvertimeRecord entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(OvertimeRecord entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(OvertimeRecord entity, CancellationToken cancellationToken = default);
}
