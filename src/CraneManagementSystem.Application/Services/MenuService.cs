using CraneManagementSystem.Application.DTOs.Menu;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;

    public MenuService(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<List<MenuDto>> GetMenusForUserByRoleAsync(string roleName, CancellationToken cancellationToken = default)
    {
        var menus = await _menuRepository.GetByRoleNameAsync(roleName, cancellationToken);
        return BuildTree(menus, null);
    }

    public async Task<List<MenuDto>> GetAllAsTreeAsync(CancellationToken cancellationToken = default)
    {
        var menus = await _menuRepository.GetAllAsync(cancellationToken);
        return BuildTree(menus.ToList(), null);
    }

    public async Task<MenuDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var menu = await _menuRepository.GetByIdAsync(id, cancellationToken);
        return menu == null ? null : MapToDto(menu, includeRoleIds: true);
    }

    public async Task<MenuDto> CreateAsync(MenuCreateDto dto, CancellationToken cancellationToken = default)
    {
        var menu = new Menu
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Icon = dto.Icon,
            Route = dto.Route,
            ParentId = dto.ParentId,
            OrderNo = dto.OrderNo,
            ModuleName = dto.ModuleName,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };
        await _menuRepository.AddAsync(menu, cancellationToken);
        if (dto.RoleIds?.Count > 0 == true)
            await _menuRepository.SetMenuRolesAsync(menu.Id, dto.RoleIds, cancellationToken);
        return MapToDto(menu, includeRoleIds: false);
    }

    public async Task<MenuDto?> UpdateAsync(Guid id, MenuUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var menu = await _menuRepository.GetByIdAsync(id, cancellationToken);
        if (menu == null) return null;

        menu.Title = dto.Title;
        menu.Icon = dto.Icon;
        menu.Route = dto.Route;
        menu.ParentId = dto.ParentId;
        menu.OrderNo = dto.OrderNo;
        menu.ModuleName = dto.ModuleName;
        menu.IsActive = dto.IsActive;
        menu.UpdatedAt = DateTime.UtcNow;
        await _menuRepository.UpdateAsync(menu, cancellationToken);
        if (dto.RoleIds != null)
            await _menuRepository.SetMenuRolesAsync(menu.Id, dto.RoleIds, cancellationToken);
        return MapToDto(menu, includeRoleIds: true);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var menu = await _menuRepository.GetByIdAsync(id, cancellationToken);
        if (menu == null) return false;
        await _menuRepository.DeleteAsync(menu, cancellationToken);
        return true;
    }

    public async Task ReorderAsync(Guid id, int newOrderNo, CancellationToken cancellationToken = default)
    {
        var menu = await _menuRepository.GetByIdAsync(id, cancellationToken);
        if (menu == null) return;
        menu.OrderNo = newOrderNo;
        menu.UpdatedAt = DateTime.UtcNow;
        await _menuRepository.UpdateAsync(menu, cancellationToken);
    }

    private static List<MenuDto> BuildTree(IReadOnlyList<Menu> flat, Guid? parentId)
    {
        return flat
            .Where(m => m.ParentId == parentId)
            .OrderBy(m => m.OrderNo)
            .ThenBy(m => m.Title)
            .Select(m => new MenuDto
            {
                Id = m.Id,
                Title = m.Title,
                Icon = m.Icon,
                Route = m.Route,
                ParentId = m.ParentId,
                OrderNo = m.OrderNo,
                ModuleName = m.ModuleName,
                IsActive = m.IsActive,
                Children = BuildTree(flat, m.Id)
            })
            .ToList();
    }

    private static MenuDto MapToDto(Menu m, bool includeRoleIds = false) => new()
    {
        Id = m.Id,
        Title = m.Title,
        Icon = m.Icon,
        Route = m.Route,
        ParentId = m.ParentId,
        OrderNo = m.OrderNo,
        ModuleName = m.ModuleName,
        IsActive = m.IsActive,
        RoleIds = includeRoleIds && m.MenuRoles?.Any() == true ? m.MenuRoles.Select(mr => mr.RoleId).ToList() : null
    };
}
