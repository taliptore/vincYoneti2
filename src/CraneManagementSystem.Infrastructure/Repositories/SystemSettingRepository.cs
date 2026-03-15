using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Infrastructure.Repositories;

public class SystemSettingRepository : ISystemSettingRepository
{
    private readonly ApplicationDbContext _context;

    public SystemSettingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SystemSetting?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SystemSettings.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<SystemSetting?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _context.SystemSettings
            .FirstOrDefaultAsync(s => s.Key == key, cancellationToken);
    }

    public async Task<IReadOnlyList<SystemSetting>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SystemSettings.ToListAsync(cancellationToken);
    }

    public async Task<SystemSetting> AddAsync(SystemSetting entity, CancellationToken cancellationToken = default)
    {
        _context.SystemSettings.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(SystemSetting entity, CancellationToken cancellationToken = default)
    {
        _context.SystemSettings.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(SystemSetting entity, CancellationToken cancellationToken = default)
    {
        _context.SystemSettings.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
