using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface IMaintenanceRecordRepository
{
    Task<MaintenanceRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MaintenanceRecord>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<MaintenanceRecord> AddAsync(MaintenanceRecord entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(MaintenanceRecord entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(MaintenanceRecord entity, CancellationToken cancellationToken = default);
}
