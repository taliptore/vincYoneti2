using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface IProgressPaymentRepository
{
    Task<ProgressPayment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProgressPayment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProgressPayment> AddAsync(ProgressPayment entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProgressPayment entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(ProgressPayment entity, CancellationToken cancellationToken = default);
}
