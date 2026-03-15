using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface IMenuRepository
{
    Task<Menu?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Menu>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Menu>> GetByRoleNameAsync(string roleName, CancellationToken cancellationToken = default);
    Task<Menu> AddAsync(Menu entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Menu entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Menu entity, CancellationToken cancellationToken = default);
    Task SetMenuRolesAsync(Guid menuId, List<Guid> roleIds, CancellationToken cancellationToken = default);
}
