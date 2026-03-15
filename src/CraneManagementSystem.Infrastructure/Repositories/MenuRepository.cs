using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Infrastructure.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly ApplicationDbContext _context;

    public MenuRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Menu?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Menus
            .Include(m => m.MenuRoles)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Menu>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Menus
            .Include(m => m.MenuRoles)
            .OrderBy(m => m.OrderNo)
            .ThenBy(m => m.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Menu>> GetByRoleNameAsync(string roleName, CancellationToken cancellationToken = default)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName, cancellationToken);
        if (role == null)
            return new List<Menu>();

        var menuIds = await _context.MenuRoles
            .Where(mr => mr.RoleId == role.Id)
            .Select(mr => mr.MenuId)
            .ToListAsync(cancellationToken);

        return await _context.Menus
            .Where(m => m.IsActive && menuIds.Contains(m.Id))
            .OrderBy(m => m.OrderNo)
            .ThenBy(m => m.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<Menu> AddAsync(Menu entity, CancellationToken cancellationToken = default)
    {
        _context.Menus.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Menu entity, CancellationToken cancellationToken = default)
    {
        _context.Menus.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Menu entity, CancellationToken cancellationToken = default)
    {
        _context.Menus.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SetMenuRolesAsync(Guid menuId, List<Guid> roleIds, CancellationToken cancellationToken = default)
    {
        var existing = await _context.MenuRoles.Where(mr => mr.MenuId == menuId).ToListAsync(cancellationToken);
        _context.MenuRoles.RemoveRange(existing);
        foreach (var roleId in roleIds.Distinct())
        {
            _context.MenuRoles.Add(new MenuRole { MenuId = menuId, RoleId = roleId });
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}
