using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface IOperatorAssignmentRepository
{
    Task<OperatorAssignment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OperatorAssignment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<OperatorAssignment> AddAsync(OperatorAssignment entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(OperatorAssignment entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(OperatorAssignment entity, CancellationToken cancellationToken = default);
}
