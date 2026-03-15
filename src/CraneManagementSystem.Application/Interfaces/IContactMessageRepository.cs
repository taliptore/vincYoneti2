using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface IContactMessageRepository
{
    Task<ContactMessage?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ContactMessage>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ContactMessage> AddAsync(ContactMessage entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(ContactMessage entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(ContactMessage entity, CancellationToken cancellationToken = default);
}
