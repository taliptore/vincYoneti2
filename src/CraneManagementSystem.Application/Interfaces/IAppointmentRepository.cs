using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Appointment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Appointment> AddAsync(Appointment entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Appointment entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Appointment entity, CancellationToken cancellationToken = default);
}
