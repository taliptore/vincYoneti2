using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface IWorkPlanRepository
{
    Task<WorkPlan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkPlan>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<WorkPlan> AddAsync(WorkPlan entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(WorkPlan entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(WorkPlan entity, CancellationToken cancellationToken = default);
}
