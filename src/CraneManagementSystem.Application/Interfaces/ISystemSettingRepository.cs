using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface ISystemSettingRepository
{
    Task<SystemSetting?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SystemSetting?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemSetting>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SystemSetting> AddAsync(SystemSetting entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(SystemSetting entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(SystemSetting entity, CancellationToken cancellationToken = default);
}
