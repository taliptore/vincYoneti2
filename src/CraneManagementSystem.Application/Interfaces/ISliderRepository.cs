using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface ISliderRepository
{
    Task<Slider?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Slider>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Slider> AddAsync(Slider entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Slider entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Slider entity, CancellationToken cancellationToken = default);
}
