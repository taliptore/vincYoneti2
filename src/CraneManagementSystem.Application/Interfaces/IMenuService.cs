using CraneManagementSystem.Application.DTOs.Menu;

namespace CraneManagementSystem.Application.Interfaces;

public interface IMenuService
{
    Task<List<MenuDto>> GetMenusForUserByRoleAsync(string roleName, CancellationToken cancellationToken = default);
    Task<List<MenuDto>> GetAllAsTreeAsync(CancellationToken cancellationToken = default);
    Task<MenuDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<MenuDto> CreateAsync(MenuCreateDto dto, CancellationToken cancellationToken = default);
    Task<MenuDto?> UpdateAsync(Guid id, MenuUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task ReorderAsync(Guid id, int newOrderNo, CancellationToken cancellationToken = default);
}
